using Chess.Application.Dtos;
using Chess.Application.Mappers;
using Chess.Data.EntityFramework;
using Chess.Engine.Board;
using Chess.Engine.Game;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EntityGame = Chess.Data.EntityFramework.Entities.Game;
using EntityMove = Chess.Data.EntityFramework.Entities.Move;

namespace Chess.Application.Services;

public sealed class GameService(ChessDbContext dbContext, ILogger<GameService> logger) : IGameService
{
    private readonly ChessDbContext _dbContext = dbContext;
    private readonly ILogger<GameService> _logger = logger;

    public async Task<GameDto?> CreateGameAsync(CreateGameDto request)
    {
        _logger.LogInformation("Creating new chess game");

        var board = new ChessBoard();
        var game = new ChessGame(board);

        if (!string.IsNullOrEmpty(request.FenPosition))
        {
            _logger.LogInformation("Loading game from FEN: {FenPosition}", request.FenPosition);

            if (!game.LoadFromFen(request.FenPosition, out var error))
            {
                _logger.LogError("Failed to load game from FEN: {Error}", error);
                throw new ArgumentException($"Invalid FEN position: {error}");
            }
        }

        var gameEntity = new EntityGame
        {
            FenPosition = game.ToFen(),
            GameState = game.State,
            GameResult = game.GameResult,
            DrawReason = game.DrawReason,
            ToMove = game.ToMove,
            FullMoveNumber = game.FullMoveNumber,
            HalfMoveClock = game.HalfMoveClock,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Games.Add(gameEntity);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Created game with ID {GameId}", gameEntity.Id);

        return GameMapper.ToDto(gameEntity);
    }

    public async Task<GameDto?> GetGameAsync(int gameId)
    {
        var gameEntity = await _dbContext.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        return gameEntity is null ? null : GameMapper.ToDto(gameEntity);
    }

    public async Task<GameDto?> MakeMoveAsync(int gameId, MakeMoveDto request)
    {
        var gameEntity = await _dbContext.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (gameEntity is null)
            return null;

        if (IsTerminal(gameEntity.GameState))
            throw new InvalidOperationException("Cannot make a move: the game is already over");

        var board = new ChessBoard();
        var game = new ChessGame(board);

        if (!game.LoadFromFen(gameEntity.FenPosition, out var fenError))
            throw new InvalidOperationException($"Game state is corrupted: {fenError}");

        var from = ParseSquare(request.FromSquare);
        var to = ParseSquare(request.ToSquare);
        var move = Move.Of(from, to);

        var colourToMove = game.ToMove;
        var moveNumber = game.FullMoveNumber;

        var result = game.TryMakeMove(move, colourToMove);

        if (!result.IsSuccess)
            throw new ArgumentException(result.Error ?? "Illegal move");

        PieceType? promotionPieceType = null;

        if (result.NewGameState == GameState.PromotionPending)
        {
            if (string.IsNullOrEmpty(request.PromotionPiece))
                throw new ArgumentException("A promotion piece must be specified (Queen, Rook, Bishop, or Knight)");

            if (!Enum.TryParse<PieceType>(request.PromotionPiece, ignoreCase: true, out var parsedPromotion))
                throw new ArgumentException($"Invalid promotion piece: '{request.PromotionPiece}'");

            result = game.CompletePromotion(parsedPromotion);
            promotionPieceType = parsedPromotion;
        }

        var newState = result.NewGameState ?? GameState.Ongoing;
        bool isCheck = newState == GameState.Check;
        bool isCheckmate = newState == GameState.Checkmate;
        string notation = game.MoveNotations.Count > 0
            ? game.MoveNotations[^1]
            : request.FromSquare + request.ToSquare;

        var moveEntity = new EntityMove
        {
            GameId = gameId,
            MoveNumber = moveNumber,
            Colour = colourToMove,
            AlgebraicNotation = notation,
            FromSquare = request.FromSquare,
            ToSquare = request.ToSquare,
            CapturedPiece = result.CapturedPiece?.Type,
            PromotionPiece = promotionPieceType,
            IsCheck = isCheck,
            IsCheckmate = isCheckmate,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Moves.Add(moveEntity);

        gameEntity.FenPosition = game.ToFen();
        gameEntity.GameState = game.State;
        gameEntity.GameResult = game.GameResult;
        gameEntity.DrawReason = game.DrawReason;
        gameEntity.ToMove = game.ToMove;
        gameEntity.FullMoveNumber = game.FullMoveNumber;
        gameEntity.HalfMoveClock = game.HalfMoveClock;
        gameEntity.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        gameEntity.Moves.Add(moveEntity);

        return GameMapper.ToDto(gameEntity);
    }

    public async Task<LegalMovesDto?> GetLegalMovesAsync(int gameId, string fromSquare)
    {
        var gameEntity = await _dbContext.Games.FindAsync(gameId);

        if (gameEntity is null)
            return null;

        if (IsTerminal(gameEntity.GameState))
            return new LegalMovesDto { FromSquare = fromSquare, LegalDestinations = [] };

        var board = new ChessBoard();
        var game = new ChessGame(board);

        if (!game.LoadFromFen(gameEntity.FenPosition, out _))
            return new LegalMovesDto { FromSquare = fromSquare, LegalDestinations = [] };

        Position from;
        try
        {
            from = ParseSquare(fromSquare);
        }
        catch (ArgumentException)
        {
            return new LegalMovesDto { FromSquare = fromSquare, LegalDestinations = [] };
        }

        var legalPositions = game.GetLegalMovesFrom(from);
        var destinations = legalPositions.Select(SquareToAlgebraic).ToList();

        return new LegalMovesDto { FromSquare = fromSquare, LegalDestinations = destinations };
    }

    public async Task<GameDto?> ResignAsync(int gameId, ResignDto request)
    {
        var gameEntity = await _dbContext.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (gameEntity is null)
            return null;

        if (IsTerminal(gameEntity.GameState))
            throw new InvalidOperationException("Cannot resign: the game is already over");

        gameEntity.GameResult = request.Colour == PieceColour.White
            ? GameResult.BlackWinsByResignation
            : GameResult.WhiteWinsByResignation;
        gameEntity.GameState = GameState.Resigned;
        gameEntity.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return GameMapper.ToDto(gameEntity);
    }

    public async Task<GameDto?> OfferDrawAsync(int gameId, DrawActionDto request)
    {
        var gameEntity = await _dbContext.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (gameEntity is null)
            return null;

        if (IsTerminal(gameEntity.GameState))
            throw new InvalidOperationException("Cannot offer draw: the game is already over");

        if (gameEntity.ToMove != request.Colour)
            throw new InvalidOperationException("Cannot offer draw: it is not your turn");

        gameEntity.DrawOfferedBy = request.Colour;
        gameEntity.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return GameMapper.ToDto(gameEntity);
    }

    public async Task<GameDto?> AcceptDrawAsync(int gameId, DrawActionDto request)
    {
        var gameEntity = await _dbContext.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (gameEntity is null)
            return null;

        if (!gameEntity.DrawOfferedBy.HasValue)
            throw new InvalidOperationException("Cannot accept draw: no draw has been offered");

        if (gameEntity.DrawOfferedBy.Value == request.Colour)
            throw new InvalidOperationException("Cannot accept your own draw offer");

        gameEntity.GameState = GameState.Draw;
        gameEntity.GameResult = GameResult.DrawByAgreement;
        gameEntity.DrawReason = DrawReason.Agreement;
        gameEntity.DrawOfferedBy = null;
        gameEntity.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return GameMapper.ToDto(gameEntity);
    }

    public async Task<GameDto?> DeclineDrawAsync(int gameId, DrawActionDto request)
    {
        var gameEntity = await _dbContext.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (gameEntity is null)
            return null;

        if (!gameEntity.DrawOfferedBy.HasValue)
            throw new InvalidOperationException("No draw offer to decline");

        gameEntity.DrawOfferedBy = null;
        gameEntity.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return GameMapper.ToDto(gameEntity);
    }

    private static bool IsTerminal(GameState state) =>
        state is GameState.Checkmate or GameState.Stalemate or GameState.Draw or GameState.Resigned;

    private static Position ParseSquare(string square)
    {
        if (square.Length != 2)
            throw new ArgumentException($"Invalid square notation: '{square}'");

        char fileChar = char.ToLower(square[0]);
        char rankChar = square[1];

        if (fileChar < 'a' || fileChar > 'h' || rankChar < '1' || rankChar > '8')
            throw new ArgumentException($"Invalid square notation: '{square}'");

        int col = fileChar - 'a';
        int row = '8' - rankChar;

        return new Position(row, col);
    }

    private static string SquareToAlgebraic(Position pos)
    {
        char file = (char)('a' + pos.Column);
        char rank = (char)('8' - pos.Row);
        return $"{file}{rank}";
    }
}

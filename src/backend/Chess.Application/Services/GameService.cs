using Chess.Application.Dtos;
using Chess.Application.Mappers;
using Chess.Data.EntityFramework;
using Chess.Data.EntityFramework.Entities;
using Chess.Engine.Board;
using Chess.Engine.Game;
using Microsoft.Extensions.Logging;

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

        var gameEntity = new Game
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

    public Task<GameDto?> GetGameAsync(int gameId)
    {
        throw new NotImplementedException();
    }

    public Task<GameDto?> MakeMoveAsync(int gameId, MakeMoveDto move)
    {
        throw new NotImplementedException();
    }
}

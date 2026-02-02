using Chess.Engine.Board;
using Chess.Engine.FenNotation;
using Chess.Engine.Helpers;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.Game;

internal class ChessGame(IChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;
    public GameState State { get; private set; } = GameState.Ongoing;
    public DrawReason? DrawReason { get; private set; } = null;

    private readonly IChessBoard _board = board;
    private readonly List<Move> _moveHistory = [];
    private readonly List<CapturedPiece> _capturedPieces = [];
    private readonly Dictionary<string, int> _positionCounts = [];
    private int _fullMoveNumber = 1;

    private int _halfMoveClock = 0;

    // Promotion state tracking
    private Move? _pendingPromotionMove = null;
    private Piece? _pendingPromotionPawn = null;
    private Piece? _pendingPromotionCapturedPiece = null;

    public IReadOnlyList<Move> MoveHistory => _moveHistory;

    public GameMoveResult TryMakeMove(Move move, PieceColour moveColour)
    {
        if (State.IsGameOver() && State != GameState.PromotionPending)
            return GameMoveResult.Illegal("The game is over");

        if (ToMove != moveColour)
            return GameMoveResult.Illegal("It's not the specified colour's turn to move");

        var moveValidationResult = MoveValidator.Validate(move, _board, ToMove);

        if (!moveValidationResult.IsLegal)
            return GameMoveResult.Illegal("Illegal move");

        var (movedPiece, capturedPiece) = MoveService.ExecuteValidatedMove(_board, move, moveValidationResult);

        UpdateCastlingRights(movedPiece, move, capturedPiece);
        UpdateEnPassantTargetSquare(move, movedPiece, moveValidationResult);
        UpdateHalfMoveClock(movedPiece, capturedPiece);

        if (ToMove == PieceColour.Black)
            _fullMoveNumber++;

        ToMove = ToMove.Opposite();

        RecordMove(move, moveValidationResult, capturedPiece);
        RecordPosition();

        if (moveValidationResult.SpecialMoveType == SpecialMoveType.Promotion)
        {
            _pendingPromotionMove = move;
            _pendingPromotionPawn = movedPiece;
            _pendingPromotionCapturedPiece = capturedPiece;
            
            State = GameState.PromotionPending;
            return GameMoveResult.RequiresPromotion(moveValidationResult, capturedPiece);
        }

        UpdateGameState(ToMove);

        return GameMoveResult.Legal(moveValidationResult, capturedPiece, State);
    }

    /// <summary>
    /// Completes a pending pawn promotion by replacing the promoted pawn with the selected piece.
    /// </summary>
    public GameMoveResult CompletePromotion(PieceType promotionPieceType)
    {
        // Validate state
        if (State != GameState.PromotionPending)
            throw new InvalidOperationException("Cannot complete promotion: no promotion is pending. Current state: " + State);

        if (promotionPieceType == PieceType.Pawn || promotionPieceType == PieceType.King)
            throw new ArgumentException($"Cannot promote pawn to {promotionPieceType}. Must be Queen, Rook, Bishop, or Knight.", nameof(promotionPieceType));

        if (_pendingPromotionMove is null || _pendingPromotionPawn is null)
            throw new InvalidOperationException("Promotion state is corrupted: missing promotion move or pawn");

        // Replace the promoted pawn with the selected piece
        var promotedPiece = new Piece(promotionPieceType, _pendingPromotionPawn.Colour);
        _board.SetPiece(_pendingPromotionMove.Value.To, promotedPiece);

        // Update game state after promotion is complete
        UpdateGameState(ToMove);

        // Prepare result
        var result = GameMoveResult.PromotionCompleted(promotionPieceType, State, _pendingPromotionCapturedPiece);

        // Clear promotion state
        _pendingPromotionMove = null;
        _pendingPromotionPawn = null;
        _pendingPromotionCapturedPiece = null;

        return result;
    }

    private void UpdateEnPassantTargetSquare(Move move, Piece movedPiece, MoveValidationResult moveValidationResult)
    {
        _board.EnPassantTargetSquare = null;

        if (moveValidationResult.IsPawnDoubleStep)
        {
            var direction = movedPiece.Colour.Direction();
            _board.EnPassantTargetSquare =
                Position.Of(move.From.Row + direction, move.From.Column);
        }
    }

    private void UpdateHalfMoveClock(Piece movedPiece, Piece? capturedPiece)
    {
        if (movedPiece.Type == PieceType.Pawn || capturedPiece != null)
            _halfMoveClock = 0;
        else
            _halfMoveClock++;
    }

    private void RecordMove(Move move, MoveValidationResult moveValidationResult, Piece? capturedPiece)
    {
        _moveHistory.Add(move);

        if (capturedPiece is not null)
        {
            _capturedPieces.Add(new CapturedPiece(_moveHistory.Count + 1, capturedPiece));
        }
    }

    private void UpdateGameState(PieceColour sideToMove)
    {
        var isInCheck = IsKingInCheck(sideToMove);
        var hasLegalMoves = HasAnyLegalMove(sideToMove);

        if (!hasLegalMoves)
        {
            if (isInCheck)
            {
                State = GameState.Checkmate;
                DrawReason = null;
            }
            else
            {
                State = GameState.Stalemate;
                DrawReason = Game.DrawReason.StaleMate;
            }
            return;
        }
        
        var currentPositionKey = PositionKeyGenerator.GeneratePositionKey(_board, sideToMove);

        if (_positionCounts.TryGetValue(currentPositionKey, out int count) && count >= 3)
        {
            State = GameState.Draw;
            DrawReason = Game.DrawReason.ThreefoldRepetition;
            return;
        }

        if (_halfMoveClock >= 100)
        {
            State = GameState.Draw;
            DrawReason = Game.DrawReason.FiftyMoveRule;
            return;
        }

        if (HasInsufficientMaterial())
        {
            State = GameState.Draw;
            DrawReason = Game.DrawReason.InsufficientMaterial;
            return;
        }

        State = isInCheck ? GameState.Check : GameState.Ongoing;
        DrawReason = null;
    }

    private bool IsKingInCheck(PieceColour colourToCheck)
    {
        var kingPos = _board.FindKing(colourToCheck);

        return _board.IsSquareAttacked(kingPos, colourToCheck.Opposite());
    }

    private bool HasAnyLegalMove(PieceColour colour)
    {
        // Loop over every square on the board (FROM squares)
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var from = Position.Of(r, c);
                var piece = _board.GetPiece(from);

                // Skip empty squares or opponent pieces
                if (piece is null || piece.Colour != colour)
                    continue;

                // Loop over every square on the board (TO squares)
                for (int tr = 0; tr < 8; tr++)
                {
                    for (int tc = 0; tc < 8; tc++)
                    {
                        var to = Position.Of(tr, tc);
                        var move = Move.Of(from, to);

                        var result = MoveValidator.Validate(move, _board, colour);

                        if (result.IsLegal)
                            return true;
                    }
                }
            }
        }

        return false;
    }

    private bool HasInsufficientMaterial()
    {
        var pieces = new List<Piece>();

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var piece = _board.GetPiece(Position.Of(r, c));
                if (piece != null)
                    pieces.Add(piece);
            }
        }

        // King vs King
        if (pieces.All(p => p.Type == PieceType.King))
            return true;

        // King + minor vs King
        if (pieces.Count == 3 &&
            pieces.Count(p => p.Type == PieceType.Bishop || p.Type == PieceType.Knight) == 1)
            return true;

        return false;
    }

    private void RecordPosition()
    {
        var key = PositionKeyGenerator.GeneratePositionKey(_board, ToMove);

        if (_positionCounts.TryGetValue(key, out int count))
            _positionCounts[key] = count + 1;
        else
            _positionCounts[key] = 1;
    }

    private void UpdateCastlingRights(
        Piece movedPiece,
        Move move,
        Piece? capturedPiece)
    {
        if (movedPiece.Type == PieceType.King)
        {
            _board.CastlingRights &= movedPiece.Colour == PieceColour.White
                ? ~(CastlingRights.WhiteKingSide | CastlingRights.WhiteQueenSide)
                : ~(CastlingRights.BlackKingSide | CastlingRights.BlackQueenSide);
        }

        if (movedPiece.Type == PieceType.Rook)
            RemoveRookCastlingRights(movedPiece.Colour, move.From);

        if (capturedPiece?.Type == PieceType.Rook)
            RemoveRookCastlingRights(capturedPiece.Colour, move.To);
    }

    private void RemoveRookCastlingRights(PieceColour colour, Position rookPosition)
    {
        if (rookPosition.Row != colour.HomeRank())
            return;

        if (rookPosition.Column == 0)
        {
            _board.CastlingRights &= colour == PieceColour.White
                ? ~CastlingRights.WhiteQueenSide
                : ~CastlingRights.BlackQueenSide;
        }
        else if (rookPosition.Column == 7)
        {
            _board.CastlingRights &= colour == PieceColour.White
                ? ~CastlingRights.WhiteKingSide
                : ~CastlingRights.BlackKingSide;
        }
    }

    /// <summary>
    /// Exports the current game state to FEN notation
    /// </summary>
    public string ToFen()
    {
        return FenUtility.ToFen(_board, ToMove, _halfMoveClock, _fullMoveNumber);
    }

    /// <summary>
    /// Loads a game state from FEN notation.
    /// </summary>
    public bool LoadFromFen(string fen, out string? error)
    {
        var result = FenUtility.FromFen(fen, _board);

        if (!result.IsSuccess)
        {
            error = result.ErrorMessage;
            return false;
        }

        // Reset game state
        ToMove = result.ToMove;
        _halfMoveClock = result.HalfMoveClock;
        _fullMoveNumber = result.FullMoveNumber;
        _moveHistory.Clear();
        _capturedPieces.Clear();
        _positionCounts.Clear();
        _pendingPromotionMove = null;
        _pendingPromotionPawn = null;
        _pendingPromotionCapturedPiece = null;

        // Record the initial position
        RecordPosition();

        // Update game state
        UpdateGameState(ToMove);

        error = null;
        return true;
    }

    public static ChessGame? FromFen(string fen, out string? error)
    {
        var board = new ChessBoard();
        var game = new ChessGame(board);

        if (!game.LoadFromFen(fen, out error))
        {
            return null;
        }

        return game;
    }
}
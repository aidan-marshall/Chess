using Chess.Engine.Board;
using Chess.Engine.Helpers;
using Chess.Engine.Moves;
using Chess.Engine.Notation;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.Game;

public class ChessGame(IChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;
    public GameState State { get; private set; } = GameState.Ongoing;
    public DrawReason? DrawReason { get; private set; } = null;

    private readonly IChessBoard _board = board;
    private readonly List<Move> _moveHistory = [];
    private readonly List<CapturedPiece> _capturedPieces = [];
    private readonly Dictionary<string, int> _positionCounts = [];
    private int _fullMoveNumber = 1;
    private readonly List<string> _moveNotations = [];
    private int _halfMoveClock = 0;
    private GameResult? _gameResult = null;
    private PieceColour? _drawOfferedBy = null;

    // Promotion state tracking
    private Move? _pendingPromotionMove = null;
    private Piece? _pendingPromotionPawn = null;
    private Piece? _pendingPromotionCapturedPiece = null;

    public IReadOnlyList<Move> MoveHistory => _moveHistory;
    public IReadOnlyList<string> MoveNotations => _moveNotations;
    public GameResult? GameResult => _gameResult;
    public PieceColour? DrawOfferedBy => _drawOfferedBy;
    public int HalfMoveClock => _halfMoveClock;
    public int FullMoveNumber => _fullMoveNumber;

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

        RecordMoveNotation(
            move,
            movedPiece,
            capturedPiece != null,
            moveValidationResult.SpecialMoveType);

        UpdateGameResult();

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

        RecordMoveNotation(
            _pendingPromotionMove.Value,
            _pendingPromotionPawn,
            _pendingPromotionCapturedPiece != null,
            SpecialMoveType.Promotion,
            promotionPieceType);

        UpdateGameResult();

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
    /// Returns all legal destination squares for the piece at the given position.
    /// Only returns moves for the side that is currently to move.
    /// </summary>
    public IReadOnlyList<Position> GetLegalMovesFrom(Position from)
    {
        var piece = _board.GetPiece(from);

        if (piece is null || piece.Colour != ToMove)
            return [];

        var legalMoves = new List<Position>();

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var to = Position.Of(r, c);
                var move = Move.Of(from, to);
                var result = MoveValidator.Validate(move, _board, ToMove);

                if (result.IsLegal)
                    legalMoves.Add(to);
            }
        }

        return legalMoves;
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
        _moveNotations.Clear();
        _pendingPromotionMove = null;
        _pendingPromotionPawn = null;
        _pendingPromotionCapturedPiece = null;
        _gameResult = null;
        _drawOfferedBy = null;

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

    /// <summary>
    /// Records the algebraic notation for a move.
    /// Call this after a move is made and UpdateGameState is called.
    /// </summary>
    private void RecordMoveNotation(
        Move move,
        Piece movedPiece,
        bool isCapture,
        SpecialMoveType specialMoveType,
        PieceType? promotionPiece = null)
    {
        bool isCastleKingSide = specialMoveType == SpecialMoveType.CastleKingSide;
        bool isCastleQueenSide = specialMoveType == SpecialMoveType.CastleQueenSide;

        // State has been updated, so check/checkmate is accurate
        bool isCheck = State == GameState.Check;
        bool isCheckmate = State == GameState.Checkmate;

        var notation = AlgebraicNotation.ToAlgebraic(
            move,
            _board,
            movedPiece,
            isCapture,
            isCheck,
            isCheckmate,
            promotionPiece,
            isCastleKingSide,
            isCastleQueenSide);

        _moveNotations.Add(notation);
    }

    /// <summary>
    /// Resigns the game for the specified color.
    /// </summary>
    /// <param name="resigningColour">The color that is resigning</param>
    public void Resign(PieceColour resigningColour)
    {
        if (State.IsGameOver() && State != GameState.PromotionPending)
        {
            throw new InvalidOperationException("Cannot resign: game is already over");
        }

        // Set the result based on who resigned
        _gameResult = resigningColour == PieceColour.White
            ? Game.GameResult.BlackWinsByResignation
            : Game.GameResult.WhiteWinsByResignation;

        State = GameState.Resigned;
    }

    /// <summary>
    /// Offers a draw from the specified color.
    /// </summary>
    /// <param name="offeringColour">The color offering the draw</param>
    public void OfferDraw(PieceColour offeringColour)
    {
        if (State.IsGameOver() && State != GameState.PromotionPending)
        {
            throw new InvalidOperationException("Cannot offer draw: game is already over");
        }

        if (ToMove != offeringColour)
        {
            throw new InvalidOperationException("Cannot offer draw: it's not your turn");
        }

        _drawOfferedBy = offeringColour;
    }

    /// <summary>
    /// Accepts a draw offer from the opponent.
    /// </summary>
    /// <param name="acceptingColour">The color accepting the draw</param>
    public void AcceptDraw(PieceColour acceptingColour)
    {
        if (!_drawOfferedBy.HasValue)
        {
            throw new InvalidOperationException("Cannot accept draw: no draw has been offered");
        }

        if (_drawOfferedBy.Value == acceptingColour)
        {
            throw new InvalidOperationException("Cannot accept your own draw offer");
        }

        // Set result and state
        _gameResult = Game.GameResult.DrawByAgreement;
        State = GameState.Draw;
        DrawReason = Game.DrawReason.Agreement;
        _drawOfferedBy = null;
    }

    /// <summary>
    /// Declines a draw offer.
    /// </summary>
    public void DeclineDraw()
    {
        _drawOfferedBy = null;
    }

    /// <summary>
    /// Updates the game result based on the current game state.
    /// Call this after UpdateGameState.
    /// </summary>
    private void UpdateGameResult()
    {
        // Don't overwrite manual results (resignations, agreements)
        if (_gameResult.HasValue)
        {
            return;
        }

        _gameResult = State switch
        {
            GameState.Checkmate => (GameResult?)(ToMove == PieceColour.White
                                ? Game.GameResult.BlackWinsByCheckmate
                                : Game.GameResult.WhiteWinsByCheckmate),// ToMove is the side that's in checkmate (just got checkmated)
                                                                        // So the OTHER side won
            GameState.Stalemate => (GameResult?)Game.GameResult.DrawByStalemate,
            GameState.Draw => DrawReason switch
            {
                Game.DrawReason.FiftyMoveRule => Game.GameResult.DrawByFiftyMoveRule,
                Game.DrawReason.ThreefoldRepetition => Game.GameResult.DrawByThreefoldRepetition,
                Game.DrawReason.InsufficientMaterial => Game.GameResult.DrawByInsufficientMaterial,
                Game.DrawReason.Agreement => Game.GameResult.DrawByAgreement,
                Game.DrawReason.StaleMate => Game.GameResult.DrawByStalemate,
                _ => null // Unknown draw reason, leave as null
            },// Map DrawReason to GameResult
            _ => null,// Ongoing, Check, PromotionPending - no result yet
        };
    }
}
using Chess.Engine.Board;
using Chess.Engine.Helpers;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine;

internal class ChessGame(IChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;
    public GameState State { get; private set; } = GameState.Ongoing;

    private readonly IChessBoard _board = board;
    private readonly List<Move> _moveHistory = [];
    private readonly List<CapturedPiece> _capturedPieces = [];

    private int _halfMoveClock = 0;

    public IReadOnlyList<Move> MoveHistory => _moveHistory;

    public GameMoveResult TryMakeMove(Move move, PieceColour moveColour)
    {
        if (State.IsGameOver())
            return GameMoveResult.Illegal("The game is over");

        if (ToMove != moveColour)
            return GameMoveResult.Illegal("It's not the specified colour's turn to move");

        var moveValidationResult = MoveValidator.Validate(move, _board, ToMove);

        if (!moveValidationResult.IsLegal)
            return GameMoveResult.Illegal("Illegal move");

        var (movedPiece, capturedPiece) = MoveService.ExecuteValidatedMove(_board, move, moveValidationResult);

        UpdateEnPassantTargetSquare(move, movedPiece, moveValidationResult);
        UpdateHalfMoveClock(movedPiece, capturedPiece);

        ToMove = ToMove.Opposite();

        RecordMove(move, moveValidationResult, capturedPiece);
        UpdateGameState();

        if (moveValidationResult.SpecialMoveType == SpecialMoveType.Promotion)
            return GameMoveResult.RequiresPromotion(moveValidationResult, capturedPiece);

        return GameMoveResult.Legal(moveValidationResult, capturedPiece, State);
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

    private void UpdateGameState()
    {
        throw new NotImplementedException();
    }
}
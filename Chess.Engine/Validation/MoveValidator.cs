using Chess.Engine.Board;
using Chess.Engine.Helpers;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine.Validation;

internal sealed class MoveValidator
{
    public static MoveValidationResult Validate(Move move, IChessBoard board, PieceColour toMove)
    {
        var movingPiece = board.GetPiece(move.From);

        if (movingPiece is null || movingPiece.Colour != toMove)
            return MoveValidationResult.Illegal();

        if (!move.To.IsWithinBounds())
            return MoveValidationResult.Illegal();

        var targetPiece = board.GetPiece(move.To);

        if (targetPiece is not null && targetPiece.Colour == movingPiece.Colour)
            return MoveValidationResult.Illegal();

        var movementPattern = MovementPatternValidator.MatchesMovementPattern(movingPiece, move);

        if (movementPattern == MovementPatternType.None)
            return MoveValidationResult.Illegal();

        var specialMoveResult = SpecialMoveValidator.Validate(movementPattern, board, movingPiece, targetPiece, move);

        if (!specialMoveResult.IsLegal)
            return MoveValidationResult.Illegal();

        if (!MovePathIsClear(move, board, movementPattern, movingPiece, specialMoveResult.CastlingRookMove?.To))
            return MoveValidationResult.Illegal();

        if (MoveLeavesKingInCheck(move, board, movingPiece.Colour, specialMoveResult))
            return MoveValidationResult.Illegal();

        if (movementPattern == MovementPatternType.PawnDoubleStep)
            specialMoveResult.IsPawnDoubleStep = true;

        if (movementPattern == MovementPatternType.PawnDoubleStep)
            {
            var enPassantCapturePosition = Position.Of(
                move.From.Row + Math.Sign(move.RowDiff),
                move.From.Column);
            return MoveValidationResult.LegalNormal(enPassantCapturePosition);
        }

        return specialMoveResult;
    }

    private static bool IsKingInCheck(
        IChessBoard board,
        PieceColour kingColour)
    {
        var kingPosition = board.FindKing(kingColour);
        return board.IsSquareAttacked(kingPosition, kingColour.Opposite());
    }

    private static bool MoveLeavesKingInCheck(
        Move move,
        IChessBoard board,
        PieceColour movingColour,
        MoveValidationResult moveValidatiotResult)
    {
        var simulatedBoard = board.Clone();

        MoveService.ExecuteValidatedMove(simulatedBoard, move, moveValidatiotResult);

        return IsKingInCheck(simulatedBoard, movingColour);
    }

    private static bool MovePathIsClear(
        Move move,
        IChessBoard board,
        MovementPatternType pattern,
        Piece movingPiece,
        Position? rookCastlePosition)
    {
        return pattern switch
        {
            MovementPatternType.Normal when movingPiece.Type is PieceType.Bishop or PieceType.Rook or PieceType.Queen
                => IsPathClear(move, board),
            MovementPatternType.PawnDoubleStep
                => IsPathClear(move, board),
            MovementPatternType.CastleKingSide or MovementPatternType.CastleQueenSide
                => IsCastlePathClear(move, rookCastlePosition, board, movingPiece),
            _ => true,
        };
    }

    private static bool IsCastlePathClear(Move kingMove, Position? rookPosition, IChessBoard board, Piece king)
    {
        if (!rookPosition.HasValue)
            throw new ApplicationException("Rook position must be provided for castling validation.");

        if (IsKingInCheck(board, king.Colour))
            return false;

        var kingToRook = Move.Of(kingMove.From, rookPosition.Value);

        if (!IsPathClear(kingToRook, board))
            return false;


        var direction = Math.Sign(kingMove.ColDiff);

        var step1 = Position.Of(kingMove.From.Row, kingMove.From.Column + direction);
        var step2 = Position.Of(kingMove.From.Row, kingMove.From.Column + 2 * direction);

        var attackingColour = king.Colour.Opposite();

        if (board.IsSquareAttacked(step1, attackingColour))
            return false;

        if (board.IsSquareAttacked(step2, attackingColour))
            return false;

        return true;
    }

    private static bool IsPathClear(Move move, IChessBoard board)
    {
        var rowStep = Math.Sign(move.RowDiff);
        var colStep = Math.Sign(move.ColDiff);

        var currentRow = move.From.Row + rowStep;
        var currentCol = move.From.Column + colStep;

        while (currentRow != move.To.Row || currentCol != move.To.Column)
        {
            var currentPosition = Position.Of(currentRow, currentCol);

            if (board.GetPiece(currentPosition) is not null)
                return false;

            currentRow += rowStep;
            currentCol += colStep;
        }

        return true;
    }
}

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









        return MoveValidationResult.LegalNormal();
    }

    private static bool DetermineIfPathClear(Move move, IChessBoard board, MovementPatternType pattern, Piece movingPiece)
    {
        return pattern switch
        {
            MovementPatternType.Normal when movingPiece.Type is PieceType.Bishop or PieceType.Rook or PieceType.Queen
                => IsPathClear(move, board),
            MovementPatternType.PawnDoubleStep
                => IsPathClear(move, board),
            MovementPatternType.CastleKingSide or MovementPatternType.CastleQueenSide
                => IsPathClear(move, board),
            _ => true,
        };
    }

    private static bool IsPathClear(Move move, IChessBoard board)
    {
        var rowStep = Math.Sign(move.RowDiff);
        var colStep = Math.Sign(move.ColDiff);

        var currentRow = move.From.Row + rowStep;
        var currentCol = move.From.Column + colStep;

        while (currentRow != move.To.Row || currentRow != move.To.Column)
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

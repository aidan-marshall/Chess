namespace Chess.Engine.Validation;

internal sealed class MoveValidator
{
    public static MoveValidationResult Validate(Move move, IChessBoard board, PieceColour toMove)
    {
        var movingPiece = board.GetPiece(move.From);

        if (movingPiece == null)
            return MoveValidationResult.Illegal();

        if (movingPiece.Colour != toMove)
            return MoveValidationResult.Illegal();

        var capturedPiece = board.GetPiece(move.To);

        if (capturedPiece is not null && capturedPiece.Colour == movingPiece.Colour)
            return MoveValidationResult.Illegal();

        if (!move.To.IsWithinBounds())
            return MoveValidationResult.Illegal();

        if (!MovementPatternValidator.MatchesMovementPattern(movingPiece, move).Matches)
            return MoveValidationResult.Illegal();







        return MoveValidationResult.LegalNormal();
    }
}

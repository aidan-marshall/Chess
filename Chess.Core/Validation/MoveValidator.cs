namespace Chess.Core.Validation;

internal sealed class MoveValidator
{
    public MoveValidationResult Validate(Move move, IChessBoard board)
    {
        var piece = board.GetPiece(move.From);

        if (piece == null)
            return MoveValidationResult.Invalid("No piece at the source position.");

        if (!MovementPatternValidator.MatchesMovementPattern(piece, move))
            return  MoveValidationResult.Invalid("The piece cannot move in that pattern.");

        return MoveValidationResult.Valid();
    }
}

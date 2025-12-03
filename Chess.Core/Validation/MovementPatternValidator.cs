namespace Chess.Engine.Validation;

internal static class MovementPatternValidator
{
    internal static bool MatchesMovementPattern(Piece piece, Move move)
    {
        return piece.Type switch
        {
            PieceType.Pawn => PawnPattern(move, piece.Colour),
            PieceType.Rook => RookPattern(move),
            PieceType.Knight => KnightPattern(move),
            PieceType.Bishop => BishopPattern(move),
            PieceType.Queen => QueenPattern(move),
            PieceType.King => KingPattern(move),
            _ => false,
        };
    }

    private static bool PawnPattern(Move move, PieceColour colour)
    {
        var direction = colour.Direction();

        if (move.ColDiff == 0)
        {
            if (move.RowDiff == direction)
                return true;

            if (move.From.Row == colour.StartingRow()
                && move.RowDiff == 2 * direction)
                return true;

            return false;
        }

        if (Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction)
        {
            return true;
        }

        return false;
    }

    private static bool RookPattern(Move move)
    {
        if (move.RowDiff == 0 && move.ColDiff != 0)
            return true;

        if (move.ColDiff == 0 && move.RowDiff != 0)
            return true;

        return false;
    }

    private static bool KnightPattern(Move move)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        // Knight must move 2 × 1 or 1 × 2
        return (rowAbs == 2 && colAbs == 1) ||
               (rowAbs == 1 && colAbs == 2);
    }

    private static bool BishopPattern(Move move)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        return rowAbs == colAbs && rowAbs != 0;
    }

    private static bool QueenPattern(Move move)
    {
        return BishopPattern(move) || RookPattern(move);
    }

    private static bool KingPattern(Move move)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        // Standard 1-square move
        if (rowAbs <= 1 && colAbs <= 1 && (rowAbs != 0 || colAbs != 0))
            return true;

        // Castling pattern
        if (rowAbs == 0 && colAbs == 2)
            return true;

        return false;
    }
}

namespace Chess.Engine.Validation;

internal static class MovementPatternValidator
{
    internal static (bool Matches, SpecialMoveType SpecialMoveType) MatchesMovementPattern(Piece piece, Move move)
    {
        return piece.Type switch
        {
            PieceType.Pawn => PawnPattern(move, piece.Colour),
            PieceType.Rook => (RookPattern(move), SpecialMoveType.None),
            PieceType.Knight => (KnightPattern(move), SpecialMoveType.None),
            PieceType.Bishop => (BishopPattern(move), SpecialMoveType.None),
            PieceType.Queen => (QueenPattern(move), SpecialMoveType.None),
            PieceType.King => KingPattern(move),
            _ => throw new ApplicationException("Piece type does not exist"),
        };
    }

    private static (bool Matches, SpecialMoveType SpecialMoveType) PawnPattern(Move move, PieceColour colour)
    {
        var direction = colour.Direction();

        var isSingleStep = move.ColDiff == 0 && move.RowDiff == direction;

        var isDoubleStep = move.ColDiff == 0
                            && move.From.Row == colour.StartingRank()
                            && move.RowDiff == 2 * direction;

        var isCapture = Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction;

        // Promotion pattern = single-step or capture forward into last rank
        var isPromotion = (isSingleStep || isCapture) &&
                           move.To.Row == colour.PromotionRank();

        if (isPromotion)
            return (true, SpecialMoveType.Promotion);

        if (isCapture)
            return (true, SpecialMoveType.Capture);

        if (isSingleStep || isDoubleStep)
            return (true, SpecialMoveType.None);

        return (false, SpecialMoveType.None);
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

    private static (bool Matches, SpecialMoveType SpecialMoveType) KingPattern(Move move)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        // Standard move: one square any direction
        if (rowAbs <= 1 && colAbs <= 1 && (rowAbs != 0 || colAbs != 0))
            return (true, SpecialMoveType.None);

        // Castling geometry
        if (rowAbs == 0 && colAbs == 2)
        {
            if (move.ColDiff > 0)
                return (true, SpecialMoveType.CastleKingSide);
            else
                return (true, SpecialMoveType.CastleQueenSide);
        }

        return (false, SpecialMoveType.None);
    }
}

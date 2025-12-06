namespace Chess.Engine.Validation;

internal static class MovementPatternValidator
{
    internal static MovementPatternType MatchesMovementPattern(Piece piece, Move move)
    {
        return piece.Type switch
        {
            PieceType.Pawn => PawnPattern(move, piece.Colour),
            PieceType.Rook => RookPattern(move),
            PieceType.Knight => KnightPattern(move),
            PieceType.Bishop => BishopPattern(move),
            PieceType.Queen => QueenPattern(move),
            PieceType.King => KingPattern(move, piece.Colour),
            _ => throw new ApplicationException("Piece type does not exist"),
        };
    }

    private static MovementPatternType PawnPattern(Move move, PieceColour colour)
    {
        var direction = colour.Direction();

        var isSingleStep = move.ColDiff == 0 && move.RowDiff == direction;

        var isDoubleStep = move.ColDiff == 0
                            && move.From.Row == colour.PawnStartingRank()
                            && move.RowDiff == 2 * direction;

        var isCapture = Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction;

        // Promotion pattern = single-step or capture forward into last rank
        var isPromotion = (isSingleStep || isCapture) &&
                           move.To.Row == colour.PromotionRank();

        if (isPromotion)
            return MovementPatternType.PawnPromotion;

        if (isCapture)
            return MovementPatternType.PawnCapture;

        if (isDoubleStep)
            return MovementPatternType.PawnDoubleStep;

        if (isSingleStep)
            return MovementPatternType.Normal;

        return MovementPatternType.None;
    }


    private static MovementPatternType RookPattern(Move move)
    {
        if (move.RowDiff == 0 && move.ColDiff != 0)
            return MovementPatternType.Normal;

        if (move.ColDiff == 0 && move.RowDiff != 0)
            return MovementPatternType.Normal;

        return MovementPatternType.None;
    }

    private static MovementPatternType KnightPattern(Move move)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        // Knight must move 2 × 1 or 1 × 2
        if ((rowAbs == 2 && colAbs == 1) || (rowAbs == 1 && colAbs == 2))
            return MovementPatternType.Normal;

        return MovementPatternType.None;
    }

    private static MovementPatternType BishopPattern(Move move)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        if (rowAbs == colAbs && rowAbs != 0)
            return MovementPatternType.Normal;

        return MovementPatternType.None;
    }

    private static MovementPatternType QueenPattern(Move move)
    {
        if (BishopPattern(move) == MovementPatternType.Normal || RookPattern(move) == MovementPatternType.Normal)
            return MovementPatternType.Normal;

        return MovementPatternType.None;
    }

    private static MovementPatternType KingPattern(Move move, PieceColour colour)
    {
        var rowAbs = Math.Abs(move.RowDiff);
        var colAbs = Math.Abs(move.ColDiff);

        // Standard move: one square any direction
        if (rowAbs <= 1 && colAbs <= 1 && (rowAbs != 0 || colAbs != 0))
            return MovementPatternType.Normal;

        // Castling geometry
        if (rowAbs == 0 && colAbs == 2)
        {

            // Check king is on starting rank
            if (move.From.Row != colour.HomeRank())
                return MovementPatternType.None;

            // Check king is on starting file
            if (move.From.Column != 4)
                return MovementPatternType.None;

            return move.ColDiff > 0
                ? MovementPatternType.CastleKingSide
                : MovementPatternType.CastleQueenSide;
        }

        return MovementPatternType.None;
    }
}

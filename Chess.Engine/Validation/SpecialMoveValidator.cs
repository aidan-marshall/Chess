namespace Chess.Engine.Validation;

internal class SpecialMoveValidator
{

    public static (bool IsLegal, SpecialMoveType SpecialMoveType) Validate(
        MovementPatternType movementPattern,
        IChessBoard board,
        Piece movingPiece,
        Piece? targetPiece,
        Move move)
    {
        return movementPattern switch
        {
            MovementPatternType.Normal => (true, SpecialMoveType.None),
            MovementPatternType.PawnCapture => PawnCapture(movingPiece, targetPiece, board, move),
            MovementPatternType.PawnDoubleStep => PawnDoubleStep(movingPiece, targetPiece),
            MovementPatternType.PawnPromotion => PawnPromotion(movingPiece, targetPiece, move),
            MovementPatternType.CastleKingSide => Castle(true),
            MovementPatternType.CastleQueenSide => Castle(false),
            _ => (false, SpecialMoveType.None)
        };
    }

    private static (bool IsLegal, SpecialMoveType SpecialMoveType) PawnCapture(
        Piece movingPiece,
        Piece? targetPiece,
        IChessBoard board,
        Move move)
    {
        // Handle Enpassant scenario
        if (targetPiece == null)
        {
            return EnPassant(
                movingPiece,
                board,
                move);
        }
        
        if (targetPiece.Colour == movingPiece.Colour)
            return (false, SpecialMoveType.None);

        return (true, SpecialMoveType.Capture);
    }

    private static (bool IsLegal, SpecialMoveType SpecialMoveType) PawnDoubleStep(Piece movingPiece, Piece? targetPiece)
    {
        var isLegal = targetPiece is null && !movingPiece.HasMoved;

        return (isLegal, SpecialMoveType.None);
    }

    private static (bool IsLegal, SpecialMoveType SpecialMoveType) PawnPromotion(
        Piece movingPiece,
        Piece? targetPiece,
        Move move)
    {
        var direction = movingPiece.Colour.Direction();

        // Must be a single-step forward OR capture forward
        var isSingleStep = move.ColDiff == 0 && move.RowDiff == direction;
        var isCapture = Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction;

        if (!isSingleStep && !isCapture)
            return (false, SpecialMoveType.None);

        // Single-step promotion must land on empty square
        if (isSingleStep && targetPiece != null)
            return (false, SpecialMoveType.None);

        // Capture promotion must capture enemy piece
        if (isCapture && (targetPiece == null || targetPiece.Colour == movingPiece.Colour))
            return (false, SpecialMoveType.None);

        return (true, SpecialMoveType.Promotion);
    }

    private static (bool IsLegal, SpecialMoveType SpecialMoveType) Castle(bool isKingSide, Piece movingPiece)
    {
        if (movingPiece.Type != PieceType.King)
            return (false, SpecialMoveType.None);

        var colour = movingPiece.Colour;
        var enemyColor = colour.Opposite();

        if (movingPiece.HasMoved)
            return (false, SpecialMoveType.None);
        



        return (false, SpecialMoveType.None);
    }



    private static (bool IsLegal, SpecialMoveType SpecialMoveType) EnPassant(
        Piece movingPiece,
        IChessBoard board,
        Move move)
    {
        if (!board.EnPassantTargetSquare.HasValue)
            return (false, SpecialMoveType.None);

        var direction = movingPiece.Colour.Direction();
        var enpassantPiecePosition = board.EnPassantTargetSquare.Value;

        // Is this an EnPassant pattern?
        if (!(Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction))
            return (false, SpecialMoveType.None);

        // Is this EnPassant move valid ?
        if (move.To != enpassantPiecePosition)
            return (false, SpecialMoveType.None);

        var enpassantPiece = board.GetPiece(Position.Of(enpassantPiecePosition.Row - direction, enpassantPiecePosition.Column));

        if (enpassantPiece is null ||
            enpassantPiece.Type != PieceType.Pawn ||
            enpassantPiece.Colour == movingPiece.Colour)
            return (false, SpecialMoveType.None);

        // Implement En Passant validation logic here
        return (true, SpecialMoveType.EnPassant);
    }
}

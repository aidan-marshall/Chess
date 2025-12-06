namespace Chess.Engine.Validation;

internal sealed record MoveValidationResult(
    bool IsLegal,
    SpecialMoveType SpecialMoveType = SpecialMoveType.None,
    Position? EnPassantCapturedPawn = null,
    Move? CastlingRookMove = null
    )
{
    public bool IsLegal { get; } = IsLegal;
    public SpecialMoveType SpecialMoveType { get; } = SpecialMoveType;
    public Position? EnPassantCapturedPawn { get; } = EnPassantCapturedPawn;
    public Move? CastlingRookMove { get; } = CastlingRookMove;

    public static MoveValidationResult Illegal()
        => new(false);

    public static MoveValidationResult LegalNormal()
        => new(true);

    public static MoveValidationResult LegalPromotion()
        => new(false, SpecialMoveType.Promotion);

    public static MoveValidationResult LegalEnPassant(Position capturedPawn)
        => new(true, SpecialMoveType.EnPassant, capturedPawn);

    public static MoveValidationResult LegalCastling(Move rookMove, bool kingSide)
        => new(true, kingSide ? SpecialMoveType.CastleKingSide : SpecialMoveType.CastleQueenSide, null, rookMove);
}

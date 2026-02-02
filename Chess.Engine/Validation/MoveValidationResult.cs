using Chess.Engine.Moves;

namespace Chess.Engine.Validation;

internal sealed record MoveValidationResult(
    bool IsLegal,
    SpecialMoveType SpecialMoveType = SpecialMoveType.None,
    Position? EnPassantCapturedPawn = null,
    Move? CastlingRookMove = null,
    bool IsPawnDoubleStep = false
    )
{
    public bool IsLegal { get; } = IsLegal;
    public SpecialMoveType SpecialMoveType { get; } = SpecialMoveType;
    public Position? EnPassantCapturedPawn { get; } = EnPassantCapturedPawn;
    public Move? CastlingRookMove { get; } = CastlingRookMove;
    public bool IsPawnDoubleStep { get; set; } = IsPawnDoubleStep;

    public static MoveValidationResult Illegal()
        => new(false);

    public static MoveValidationResult LegalNormal(bool isPawnDoubleStep = false)
        => new(true, IsPawnDoubleStep: isPawnDoubleStep);

    public static MoveValidationResult LegalPromotion()
        => new(true, SpecialMoveType.Promotion);

    public static MoveValidationResult LegalEnPassant(Position capturedPawn)
        => new(true, SpecialMoveType.EnPassant, capturedPawn);

    public static MoveValidationResult LegalCastling(Move rookMove, bool kingSide)
        => new(true, kingSide ? SpecialMoveType.CastleKingSide : SpecialMoveType.CastleQueenSide, null, rookMove);
}

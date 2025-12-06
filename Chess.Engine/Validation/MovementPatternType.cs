namespace Chess.Engine.Validation;

internal enum MovementPatternType
{
    None,
    Normal,
    PawnCapture,
    PawnDoubleStep,
    PawnPromotion,
    CastleKingSide,
    CastleQueenSide
}

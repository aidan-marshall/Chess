namespace Chess.Engine.Game;

internal enum DrawReason
{
    StaleMate,
    FiftyMoveRule,
    ThreefoldRepetition,
    InsufficientMaterial,
    Agreement,
    DeadPosition,
    TimeoutInsufficientMaterial
}

namespace Chess.Engine.Game;

public enum DrawReason
{
    StaleMate,
    FiftyMoveRule,
    ThreefoldRepetition,
    InsufficientMaterial,
    Agreement,
    DeadPosition,
    TimeoutInsufficientMaterial
}

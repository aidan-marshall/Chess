namespace Chess.Engine.Game;

public enum GameResult
{
    WhiteWinsByCheckmate,
    BlackWinsByCheckmate,
    WhiteWinsByResignation,
    BlackWinsByResignation,
    DrawByStalemate,
    DrawByFiftyMoveRule,
    DrawByThreefoldRepetition,
    DrawByInsufficientMaterial,
    DrawByAgreement
}

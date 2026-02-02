using Chess.Engine.Game;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.Helpers;

internal static class Extensions
{
    public static PieceColour Opposite(this PieceColour colour) => colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

    public static int Direction(this PieceColour colour) => colour == PieceColour.White ? -1 : 1;

    public static int PawnStartingRank(this PieceColour colour) => colour == PieceColour.White ? 6 : 1;
    
    public static int HomeRank(this PieceColour colour) => colour == PieceColour.White ? 7 : 0;

    public static int PromotionRank(this PieceColour colour) => colour == PieceColour.White ? 0 : 7;

    public static bool IsGameOver(this GameState gameStatus) =>
        gameStatus == GameState.Checkmate ||
        gameStatus == GameState.Stalemate ||
        gameStatus == GameState.Draw;
}

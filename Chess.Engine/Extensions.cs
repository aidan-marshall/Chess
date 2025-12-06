namespace Chess.Engine;

public static class Extensions
{
    public static PieceColour Opposite(this PieceColour colour) => colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

    public static int Direction(this PieceColour colour) => colour == PieceColour.White ? -1 : 1;

    public static int PawnStartingRank(this PieceColour colour) => colour == PieceColour.White ? 6 : 1;
    
    public static int HomeRank(this PieceColour colour) => colour == PieceColour.White ? 7 : 0;

    public static int PromotionRank(this PieceColour colour) => colour == PieceColour.White ? 0 : 7;
}

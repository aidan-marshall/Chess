namespace Chess.Core;

public static class Extensions
{
    public static PieceColour Opposite(this PieceColour colour) => colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

    public static int Direction(this PieceColour colour) => colour == PieceColour.White ? -1 : 1;

    public static int StartingRow(this PieceColour colour) => colour == PieceColour.White ? 6 : 1;
}

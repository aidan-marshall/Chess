using Chess.Core.Pieces;

namespace Chess.Core;

public static class Extensions
{
    public static PieceColour Opposite(this PieceColour colour) => colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
}

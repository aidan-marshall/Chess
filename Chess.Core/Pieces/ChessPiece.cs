namespace Chess.Core.Pieces;

public abstract class ChessPiece(PieceColour colour)
{
    private PieceColour Colour { get; } = colour;
    public abstract PieceType Type { get; }

    public abstract bool ValidMove(ChessMove move, ChessBoard board);
}
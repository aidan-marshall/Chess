namespace Chess.Core.Pieces;



public abstract class ChessPiece(PieceColour colour) : IChessPiece
{
    public PieceColour Colour { get; } = colour;
    public abstract PieceType Type { get; }

    public abstract bool ValidMove(ChessMove move, IChessBoard board);
    
    public virtual void FinalizeTurn()
    {
    }
}
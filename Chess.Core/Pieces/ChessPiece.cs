namespace Chess.Core.Pieces;

public abstract class ChessPiece(PieceColour colour)
{
    public PieceColour Colour { get; } = colour;
    public int MovedAmount { get; set; } = 0;
    public bool HasMoved => MovedAmount > 0;

    public abstract bool ValidMove(ChessMove move, IChessBoard board);
    
    public virtual void FinalizeTurn()
    {
    }
}
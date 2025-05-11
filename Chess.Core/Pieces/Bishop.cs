namespace Chess.Core.Pieces;

public class Bishop(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Bishop;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        throw new NotImplementedException();
    }
}
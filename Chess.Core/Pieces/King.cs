namespace Chess.Core.Pieces;

public class King(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.King;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        throw new NotImplementedException();
    }
}
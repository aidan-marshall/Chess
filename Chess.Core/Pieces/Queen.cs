namespace Chess.Core.Pieces;

public class Queen(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Queen;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        throw new NotImplementedException();
    }
}
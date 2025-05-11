namespace Chess.Core.Pieces;

public class Rook(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Rook;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        throw new NotImplementedException();
    }
}
namespace Chess.Core.Pieces;

public class Knight(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Knight;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        throw new NotImplementedException();
    }
}
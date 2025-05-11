namespace Chess.Core.Pieces;

public class Pawn(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Pawn;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        throw new NotImplementedException();
    }
}
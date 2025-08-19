namespace Chess.Core.Pieces;

public class Knight(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Knight;

    public override bool ValidMove(ChessMove move, IChessBoard board)
    {
        var (fromRow, fromCol) = move.From;
        var (toRow, toCol) = move.To;

        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);

        // The only two valid combinations for an 'L' shape are (1, 2) and (2, 1).
        bool isLShape = (rowDiff == 1 && colDiff == 2) || (rowDiff == 2 && colDiff == 1);

        if (!isLShape)
        {
            return false;
        }

        var targetPiece = board.GetPiece(move.To);

        // A knight can move to an empty square or capture an enemy piece.
        // It cannot move to a square occupied by a friendly piece.
        if (targetPiece != null && targetPiece.Colour == this.Colour)
        {
            return false;
        }

        return true;
    }
}
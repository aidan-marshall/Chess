namespace Chess.Core.Pieces;

public class Rook(PieceColour colour) : ChessPiece(colour)
{
    public override bool ValidMove(Move move, IChessBoard board)
    {
        var (fromRow, fromCol) = move.From.ToTuple();
        var (toRow, toCol) = move.To.ToTuple();
        
        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);

        // If it's not a straight line, it's not a valid rook move.
        // Also, it must actually move from its square.
        if ((rowDiff > 0 && colDiff > 0) || (rowDiff == 0 && colDiff == 0))
        {
            return false;
        }

        // Path Check: The path must be clear ---
        // Determine the direction of movement. Math.Sign returns -1, 0, or 1.
        int rowStep = Math.Sign(toRow - fromRow);
        int colStep = Math.Sign(toCol - fromCol);

        int currentRow = fromRow + rowStep;
        int currentCol = fromCol + colStep;

        // Iterate through each square on the path between the start and end.
        while (currentRow != toRow || currentCol != toCol)
        {
            if (board.GetPiece(new Position(currentRow, currentCol)) != null)
            {
                return false;
            }
            currentRow += rowStep;
            currentCol += colStep;
        }

        // Destination Check: Cannot capture a friendly piece
        var targetPiece = board.GetPiece(move.To);
        if (targetPiece != null && targetPiece.Colour == this.Colour)
        {
            return false;
        }

        return true;
    }
}
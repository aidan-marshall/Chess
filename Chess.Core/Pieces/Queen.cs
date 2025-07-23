namespace Chess.Core.Pieces;

public class Queen(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Queen;

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        var (fromRow, fromCol) = move.From;
        var (toRow, toCol) = move.To;

        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);

        bool isStraight = rowDiff == 0 || colDiff == 0;
        bool isDiagonal = rowDiff == colDiff;

        // If the move is neither straight nor diagonal, it's invalid.
        // Also, it must actually move from its square.
        if ((!isStraight && !isDiagonal) || (rowDiff == 0 && colDiff == 0))
        {
            return false;
        }

        // Path Check: The path must be clear ---
        // Determine the direction of movement.
        int rowStep = Math.Sign(toRow - fromRow);
        int colStep = Math.Sign(toCol - fromCol);

        int currentRow = fromRow + rowStep;
        int currentCol = fromCol + colStep;

        // Iterate through each square on the path between the start and end.
        while (currentRow != toRow || currentCol != toCol)
        {
            if (board.GetPiece((currentRow, currentCol)) != null)
            {
                return false; // The path is blocked.
            }
            currentRow += rowStep;
            currentCol += colStep;
        }

        // --- 3. Destination Check: Cannot capture a friendly piece ---
        var targetPiece = board.GetPiece(move.To);
        if (targetPiece != null && targetPiece.Colour == this.Colour)
        {
            return false;
        }

        return true;
    }
}
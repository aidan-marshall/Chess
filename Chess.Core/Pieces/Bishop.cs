namespace Chess.Core.Pieces;

public class Bishop(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.Bishop;

    public override bool ValidMove(ChessMove move, IChessBoard board)
    {
        var (fromRow, fromCol) = move.From;
        var (toRow, toCol) = move.To;

        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);

        // For a diagonal move, the absolute change in rows must equal the absolute change in columns.
        // If not, it's an invalid move for a bishop.
        if (rowDiff != colDiff || rowDiff == 0)
        {
            return false;
        }

        // Path Check: The path must be clear
        // Determine the direction of movement (e.g., up-right, down-left).
        int rowStep = (toRow > fromRow) ? 1 : -1;
        int colStep = (toCol > fromCol) ? 1 : -1;

        int currentRow = fromRow + rowStep;
        int currentCol = fromCol + colStep;

        // Iterate through each square on the path between the start and end.
        while (currentRow != toRow)
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

        // If all checks pass, the move is pseudo-legal.
        return true;
    }
}
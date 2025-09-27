namespace Chess.Core.Pieces;

public class King(PieceColour colour) : ChessPiece(colour)
{
    /// <summary>
    /// Validates a pseudo-legal move for a King.
    /// This method identifies the King's basic movement patterns but does NOT validate
    /// rules that require global board context, such as:
    /// - Moving into check.
    /// - Castling through or out of check.
    /// - Castling when rights have been lost.
    /// These complex rules will be validated by the main ChessGame class.
    /// </summary>
    /// <param name="move">The move to validate.</param>
    /// <param name="board">The current state of the chessboard.</param>
    /// <returns>True if the move matches a king's movement pattern, otherwise false.</returns>
    public override bool ValidMove(Move move, IChessBoard board)
    {
        var (fromRow, fromCol) = move.From.ToTuple();
        var (toRow, toCol) = move.To.ToTuple();

        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);

        // --- 1. Standard Move: One square in any direction ---
        bool isStandardMove = Math.Max(rowDiff, colDiff) == 1;
        if (isStandardMove)
        {
            var targetPiece = board.GetPiece(move.To);
            if (targetPiece != null && targetPiece.Colour == this.Colour)
            {
                return false;
            }
            return true;
        }

        // --- 2. Castling Attempt: Two squares horizontally FROM THE STARTING SQUARE ---
        // ChessGame class will handle the complex rules of castling,
        var isCastlePattern = rowDiff == 0 && colDiff == 2;
        if (isCastlePattern)
        {
            if (!HasMoved)
            {
                return true;
            }
        }

        // If the move is not a standard 1-square move or a valid castle attempt pattern, it's invalid.
        return false;
    }

}
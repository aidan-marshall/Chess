namespace Chess.Core.Pieces;

public class Pawn(PieceColour _) : ChessPiece(_)
{
    public override PieceType Type => PieceType.Pawn;

    /// <summary>
    /// Validates a pseudo-legal move for a Pawn.
    /// This method checks for all of a pawn's unique movement patterns but does NOT
    /// validate for check or pins. It relies on the board state for en passant.
    /// </summary>
    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        // Determine pawn direction and starting row based on color.
        int direction = this.Colour == PieceColour.White ? -1 : 1; // White moves from row 6->0, Black from 1->7
        int startRow = this.Colour == PieceColour.White ? 6 : 1;

        var (fromRow, fromCol) = move.From;
        var (toRow, toCol) = move.To;

        // --- 1. Forward Moves ---
        if (fromCol == toCol)
        {
            // Standard one-square move
            if (toRow == fromRow + direction && board.GetPiece(move.To) == null)
            {
                return true;
            }

            // Initial two-square move
            if (fromRow == startRow && toRow == fromRow + 2 * direction && board.GetPiece(move.To) == null)
            {
                // The path must also be clear (the square being jumped over)
                var jumpedSquare = (fromRow + direction, fromCol);
                if (board.GetPiece(jumpedSquare) == null)
                {
                    return true;
                }
            }
        }

        // --- 2. Diagonal Moves (Capture or En Passant) ---
        if (Math.Abs(toCol - fromCol) == 1 && toRow == fromRow + direction)
        {
            // Standard capture
            var targetPiece = board.GetPiece(move.To);
            if (targetPiece != null && targetPiece.Colour != this.Colour)
            {
                return true;
            }

            // En Passant capture
            if (move.To == board.EnPassantTargetSquare)
            {
                return true;
            }
        }

        return false;
    }
}
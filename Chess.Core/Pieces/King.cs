namespace Chess.Core.Pieces;

public class King(PieceColour colour) : ChessPiece(colour)
{
    public override PieceType Type => PieceType.King;

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
    public override bool ValidMove(ChessMove move, IChessBoard board)
    {
        var (fromRow, fromCol) = move.From;
        var (toRow, toCol) = move.To;

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
        bool isCastlePattern = rowDiff == 0 && colDiff == 2;
        if (isCastlePattern)
        {
            // A castling pattern is only a valid pseudo-move if the king is on its starting square.
            bool isWhiteKingOnStart = this.Colour == PieceColour.White && fromRow == 7 && fromCol == 4;
            bool isBlackKingOnStart = this.Colour == PieceColour.Black && fromRow == 0 && fromCol == 4;

            if (isWhiteKingOnStart || isBlackKingOnStart)
            {
                return true;
            }
        }

        // If the move is not a standard 1-square move or a valid castle attempt pattern, it's invalid.
        return false;
    }
}
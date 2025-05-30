namespace Chess.Core.Pieces;

public class Pawn(PieceColour _) : ChessPiece(_)
{
    public override PieceType Type => PieceType.Pawn;
    
    public bool EnPassantAllowed { get; private set; }

    public override bool ValidMove(ChessMove move, ChessBoard board)
    {
        var direction = Colour == PieceColour.White ? 1 : -1;
        var startingRow = Colour == PieceColour.White ? 6 : 1;
        
        // Forward move
        if (move.From.col == move.To.col)
        {
            var moveDistance = move.From.row - move.To.row;
            
            // Normal one square forward move
            if (moveDistance == direction && board.GetPiece(move.To) == null)
                return true;
                
            // The first move can be two squares
            if (move.From.row == startingRow && moveDistance == direction * 2 
                && board.GetPiece(move.To) == null 
                && board.GetPiece((move.From.row - direction, move.To.col)) == null)
                return true;
        }
        
        // Capture moves
        if (Math.Abs(move.From.col - move.To.col) != 1 || (move.From.row - move.To.row) != direction) return false;
        var targetPiece = board.GetPiece(move.To);
        if (targetPiece != null && targetPiece.Colour != Colour)
            return true;
            
        // En passant capture
        if (targetPiece != null) return false;
        // Check for en passant - get the pawn that might be captured
        var enPassantTarget = board.GetPiece((move.From.row, move.To.col));

        // Check if the piece is a pawn of the opposite color and eligible for en passant
        return enPassantTarget is Pawn enemyPawn && 
               enemyPawn.Colour != Colour && 
               enemyPawn.EnPassantAllowed;
    }
    
    public override void FinalizeTurn()
    {
        // Reset en passant eligibility at the end of the turn
        EnPassantAllowed = false;
    }
}
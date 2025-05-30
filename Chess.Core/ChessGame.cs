using Chess.Core.Pieces;

namespace Chess.Core;

public class ChessGame(ChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;

    private ChessBoard Board { get; set; } = board;

    public bool MovePiece(ChessMove move)
    {
        var piece = Board.GetPiece(move.From);
                
        if (piece == null || piece.Colour != ToMove)
        {
            return false;
        }
        
        if (!piece.ValidMove(move, Board))
        {
            return false;
        }
        
        ToMove = ToMove == PieceColour.White ? PieceColour.Black : PieceColour.White;
        
        return true;
    }
}
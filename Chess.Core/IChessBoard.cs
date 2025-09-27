using Chess.Core.Pieces;

namespace Chess.Core;

public interface IChessBoard
{
    ChessPiece?[,] Board { get; }
    ChessMove? LastMove { get; }
    public (int, int)? EnPassantTargetSquare { get; set; }
    ChessPiece? GetPiece((int row, int col) position);
    void SetPiece((int row, int col) position, ChessPiece? piece);
    (int, int) FindKing(PieceColour kingColour);
    ChessPiece PerformMove(ChessMove move);
    void Clear();
    bool HasKingMoved((int row, int col) position, King king);
    bool HasRookMoved(bool isQueenSide, PieceColour colourToFetch);
    bool IsSquareAttacked((int, int) position, PieceColour colour);
}
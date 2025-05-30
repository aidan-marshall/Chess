using Chess.Core.Pieces;

namespace Chess.Core;

public interface IChessBoard
{
    ChessPiece?[,] Board { get; }
    ChessMove? LastMove { get; }
    ChessPiece? GetPiece((int row, int col) position);
    void SetPiece((int row, int col) position, ChessPiece piece);
}
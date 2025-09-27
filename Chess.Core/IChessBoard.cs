using Chess.Core.Pieces;

namespace Chess.Core;

public interface IChessBoard
{
    ChessPiece?[,] Board { get; }
    Move? LastMove { get; }
    public Position? EnPassantTargetSquare { get; set; }
    ChessPiece? GetPiece(Position position);
    void SetPiece(Position position, ChessPiece? piece);
    Position FindKing(PieceColour kingColour);
    ChessPiece PerformMove(Move move);
    void Clear();
    bool IsSquareAttacked(Position position, PieceColour attackingColour);
}
using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine;

internal interface IChessBoard
{
    Position? EnPassantTargetSquare { get; set; }
    Piece? GetPiece(Position position);
    void SetPiece(Position position, Piece? piece);
    void Clear();
    Position FindKing(PieceColour kingColour);
    bool IsSquareAttacked(Position position, PieceColour attackingColour);
    bool IsPathClear(Move move);
    IChessBoard Clone();
}
namespace Chess.Core;

internal interface IChessBoard
{
    Move? LastMove { get; }
    Position? EnPassantTargetSquare { get; set; }
    Piece? GetPiece(Position position);
    void SetPiece(Position position, Piece? piece);
    Position FindKing(PieceColour kingColour);
    Piece PerformMove(Move move);
    void Clear();
    bool IsSquareAttacked(Position position, PieceColour attackingColour);
}
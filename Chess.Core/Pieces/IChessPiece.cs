namespace Chess.Core.Pieces;

public interface IChessPiece
{
    PieceColour Colour { get; }
    PieceType Type { get; }

    bool ValidMove(ChessMove move, IChessBoard board);
    void FinalizeTurn();
}
namespace Chess.Core.Pieces;

public interface IChessPiece
{
    PieceColour Colour { get; }
    PieceType Type { get; }

    bool ValidMove(ChessMove move, ChessBoard board);
    void FinalizeTurn();
}
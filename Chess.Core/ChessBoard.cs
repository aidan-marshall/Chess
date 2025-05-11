using Chess.Core.Pieces;

namespace Chess.Core;

public class ChessBoard
{
    public ChessPiece?[,] Board { get; private set; } = new ChessPiece?[8, 8];

    public ChessBoard()
    {
        Setup();
    }

    public ChessPiece? GetPiece((int row, int col) position)
    {
        return Board[position.row, position.col];
    }

    public void SetPiece((int row, int col) position, ChessPiece piece)
    {
        Board[position.row, position.col] = piece;
    }

    private void Setup()
    {
        SetupBackRank(PieceColour.Black);
        // Setup black pawns
        for (var col = 0; col < 8; col++)
        {
            Board[col, 1]  = new Pawn(PieceColour.Black);
        }

        SetupBackRank(PieceColour.White);
        // Setup white pawns
        for (var col = 0; col < 8; col++)
        {
            Board[col, 6] = new Pawn(PieceColour.White);
        }
    }

    private void SetupBackRank(PieceColour colour)
    {
        var row = colour == PieceColour.White ? 7 : 0;
        Board[row, 0] = new Rook(colour);
        Board[row, 1] = new Knight(colour);
        Board[row, 2] = new Bishop(colour);
        Board[row, 3] = new Queen(colour);
        Board[row, 4] = new King(colour);
        Board[row, 5] = new Bishop(colour);
        Board[row, 6] = new Knight(colour);
        Board[row, 7] = new Rook(colour);
    }
}
using Chess.Core.Pieces;

namespace Chess.Core;

public class ChessBoard : IChessBoard
{
    ChessPiece?[,] IChessBoard.Board { get => Board; }
    public ChessPiece?[,] Board { get; private set; } = new ChessPiece?[8, 8];

    ChessMove? IChessBoard.LastMove { get => LastMove; }
    public ChessMove? LastMove { get; private set; }
    public CastlingRights CastlingRights { get; private set; }
    public (int, int)? EnPassantTargetSquare { get; set; }

    public ChessBoard()
    {
        Setup();
        CastlingRights = CastlingRights.All;
    }

    public ChessPiece? GetPiece((int row, int col) position)
    {
        return Board[position.row, position.col];
    }

    public void SetPiece((int row, int col) position, ChessPiece? piece)
    {
        Board[position.row, position.col] = piece;

        if (piece is not null)
            piece.MovedAmount++;
    }

    private void Setup()
    {

        SetupBackRank(PieceColour.Black);
        // Setup black pawns
        for (var col = 0; col < 8; col++)
        {
            Board[1, col] = new Pawn(PieceColour.Black);
        }

        SetupBackRank(PieceColour.White);
        // Setup white pawns
        for (var col = 0; col < 8; col++)
        {
            Board[6, col] = new Pawn(PieceColour.White);
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

    public void Clear()
    {
        Board = new ChessPiece?[8, 8];
    }

    public (int, int) FindKing(PieceColour kingColour)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var piece = GetPiece((r, c));
                if (piece is King && piece.Colour == kingColour)
                {
                    return (r, c);
                }
            }
        }

        throw new Exception("King not found on the board.");
    }

    public ChessPiece PerformMove(ChessMove move)
    {
        var piece = GetPiece(move.From) ?? throw new InvalidOperationException("No piece at the source square.");
        SetPiece(move.To, piece);
        SetPiece(move.From, null);

        return piece;
    }

    public bool IsSquareAttacked((int, int) position, PieceColour colour)
    {
        throw new NotImplementedException();
    }
}
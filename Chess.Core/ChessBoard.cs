using Chess.Core.Pieces;

namespace Chess.Core;

public class ChessBoard : IChessBoard
{
    ChessPiece?[,] IChessBoard.Board { get => Board; }
    public ChessPiece?[,] Board { get; private set; } = new ChessPiece?[8, 8];
    Move? IChessBoard.LastMove => Moves.LastOrDefault();
    public List<Move> Moves { get; } = [];
    public Move? LastMove { get; private set; }
    public Position? EnPassantTargetSquare { get; set; }

    public ChessBoard()
    {
        Setup();
    }

    public ChessPiece? GetPiece(Position position)
    {
        return Board[position.Row, position.Column];
    }

    public void SetPiece(Position position, ChessPiece? piece)
    {
        Board[position.Row, position.Column] = piece;


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

    public Position FindKing(PieceColour kingColour)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var position = new Position(r, c);
                var piece = GetPiece(position);
                if (piece is King && piece.Colour == kingColour)
                {
                    return position;
                }
            }
        }

        throw new Exception("King not found on the board.");
    }

    public ChessPiece PerformMove(Move move)
    {
        var piece = GetPiece(move.From) ?? throw new InvalidOperationException("No piece at the source square.");
        SetPiece(move.To, piece);
        SetPiece(move.From, null);

        piece.MovedAmount++;
        Moves.Add(move);

        return piece;
    }

    public bool IsSquareAttacked(Position attackedPosition, PieceColour attackingColour)
    {
        for (var r = 0; r < 8; r++)
        {
            for (var c = 0; c < 8; c++)
            {
                var currentPosition = new Position(r, c);

                var piece = GetPiece(currentPosition);

                if (piece == null || piece.Colour != attackingColour) continue;

                var move = new Move(currentPosition, attackedPosition);

                if (piece.ValidMove(move, this)) return true;
            }
        }

        return false;
    }
}
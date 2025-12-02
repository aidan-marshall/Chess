namespace Chess.Core;

internal class ChessBoard : IChessBoard
{
    private Piece?[,] _board;
    public List<Move> Moves { get; } = [];
    Move? IChessBoard.LastMove => Moves.LastOrDefault();
    public Position? EnPassantTargetSquare { get; set; }

    public ChessBoard()
    {
        _board = new Piece?[8, 8];
        Setup();
    }

    public Piece? GetPiece(Position position)
    {
        return _board[position.Row, position.Column];
    }

    public void SetPiece(Position position, Piece? piece)
    {
        _board[position.Row, position.Column] = piece;
    }

    private void Setup()
    {

        SetupBackRank(PieceColour.Black);
        // Setup black pawns
        for (var col = 0; col < 8; col++)
        {
            _board[1, col] = Piece.Pawn(PieceColour.Black);
        }

        SetupBackRank(PieceColour.White);
        // Setup white pawns
        for (var col = 0; col < 8; col++)
        {
            _board[6, col] = Piece.Pawn(PieceColour.White);
        }
    }

    private void SetupBackRank(PieceColour colour)
    {
        var row = colour == PieceColour.White ? 7 : 0;
        _board[row, 0] = Piece.Rook(colour);
        _board[row, 1] = Piece.Knight(colour);
        _board[row, 2] = Piece.Bishop(colour);
        _board[row, 3] = Piece.Queen(colour);
        _board[row, 4] = Piece.King(colour);
        _board[row, 5] = Piece.Bishop(colour);
        _board[row, 6] = Piece.Knight(colour);
        _board[row, 7] = Piece.Rook(colour);
    }

    public void Clear()
    {
        _board = new Piece?[8, 8];
    }

    public Position FindKing(PieceColour kingColour)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var position = new Position(r, c);
                var piece = GetPiece(position);

                if (piece?.Type == PieceType.King && piece.Colour == kingColour)
                {
                    return position;
                }
            }
        }

        throw new Exception("King not found on the board.");
    }

    public Piece PerformMove(Move move)
    {
        var piece = GetPiece(move.From) ?? throw new InvalidOperationException("No piece at the source square.");
        SetPiece(move.To, piece);
        SetPiece(move.From, null);

        piece.MoveAmount++;
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

                //if (piece.ValidMove(move, this)) return true;
                return true; // temp
            }
        }

        return false;
    }
}
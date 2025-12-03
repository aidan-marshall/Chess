namespace Chess.Engine;

public enum GameStatus
{
    Ongoing,
    Check,
    Checkmate,
    Stalemate
}

internal class ChessGame(IChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;

    public GameStatus Status { get; private set; } = GameStatus.Ongoing;

    private readonly IChessBoard _board = board;

    public MoveResult MakeMove(Move move)
    {
        var piece = _board.GetPiece(move.From);

        var capturedPiece = _board.GetPiece(move.To);

        if (piece == null)
            return MoveResult.PieceNotFound;

        if (piece.Colour != ToMove)
            return MoveResult.PlayerOutOfTurn;

        if (!move.To.IsWithinBounds())
            return MoveResult.IllegalMove;

        //if (!piece.ValidMove(move, _board))
        //    return MoveResult.IllegalMove;

        //if (!IsCastleLegal(move, piece))
        //    return MoveResult.IllegalMove;

        if (IsKingInCheck(move))
            return MoveResult.MoveLeavesKingInCheck;


        return MoveResult.Success;
    }

    private bool IsKingInCheck(Move move)
    {
        var piece = _board.GetPiece(move.From);
        var capturedPiece = _board.GetPiece(move.To);

        _board.PerformMove(move);

        var kingPosition = _board.FindKing(ToMove);

        var isInCheck = _board.IsSquareAttacked(kingPosition, ToMove.Opposite());

        _board.PerformMove(new Move(move.To, move.From));
        if (capturedPiece != null)
        {
            _board.SetPiece(move.To, capturedPiece);
        }

        return isInCheck;
    }

    //private bool IsCastleLegal(Move move, ChessPiece piece)
    //{
    //    if (!(piece is King king && Math.Abs(move.To.Column - move.From.Column) == 2))
    //        return true;

    //    var rowDiff = Math.Abs(move.To.Row - move.From.Row);
    //    var colDiff = Math.Abs(move.To.Column - move.From.Column);

    //    var isCastlePattern = rowDiff == 0 && colDiff == 2;

    //    if (!isCastlePattern)
    //    {
    //        return false;
    //    }

    //    if (king.HasMoved)
    //    {
    //        return false;
    //    }

    //    // Determine which rook to try and fetch
    //    var isQueenSide = king.Colour == PieceColour.White
    //        ? move.To.Column < move.From.Column
    //        : move.To.Column > move.From.Column;

    //    var rowToFetch = king.Colour == PieceColour.White ? 7 : 0;
    //    var colToFetch = isQueenSide ? 0 : 7;

    //    var rookStartingPosition = new Position(rowToFetch, colToFetch);

    //    var rookCastling = _board.GetPiece(rookStartingPosition);

    //    if (rookCastling is not Rook rook) return false;

    //    if (rook.Colour != king.Colour) return false;

    //    if (rook.HasMoved) return false;

    //    var step = isQueenSide ? -1 : 1;

    //    // determine if path is clear
    //    for (var col = move.From.Column + step; col != colToFetch + step; col += step)
    //    {
    //        var position = new Position(move.From.Row, col);

    //        if (_board.GetPiece(position) is not null)
    //            return false;

    //        if (col != colToFetch && _board.IsSquareAttacked(position, king.Colour.Opposite()))
    //            return false ;
    //    }

    //    return true;
    //}
}
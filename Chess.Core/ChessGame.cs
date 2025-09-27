using Chess.Core.Pieces;

namespace Chess.Core;

public enum GameStatus { Ongoing, Check, Checkmate, Stalemate }
public class ChessGame(IChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;
    public GameStatus Status { get; private set; } = GameStatus.Ongoing;
    private readonly IChessBoard _board = board;

    public MoveResult MakeMove(Move move)
    {
        var piece = _board.GetPiece(move.From);

        if (piece == null)
            return MoveResult.PieceNotFound;

        if (piece.Colour != ToMove)
            return MoveResult.PlayerOutOfTurn;

        if (!piece.ValidMove(move, _board))
        {
            return MoveResult.IllegalMove;
        }

        if (!IsMoveFullyLegal(move, piece))
            return MoveResult.MoveLeavesKingInCheck;

        return MoveResult.Success;
    }

    private bool IsMoveFullyLegal(Move move, ChessPiece piece)
    {
        if (piece is King king && Math.Abs(move.To.Column - move.From.Column) == 2)
            return IsCastleLegal(move, king);

        return IsKingInCheck(move);
    }

    private bool IsKingInCheck(Move move)
    {
        throw new NotImplementedException();
    }

    private bool IsCastleLegal(Move move, King king)
    {
        var rowDiff = Math.Abs(move.To.Row - move.From.Row);
        var colDiff = Math.Abs(move.To.Column - move.From.Column);

        var isCastlePattern = rowDiff == 0 && colDiff == 2;

        if (!isCastlePattern)
        {
            return false;
        }

        if (king.HasMoved)
        {
            return false;
        }

        // Determine which rook to try and fetch
        var isQueenSide = king.Colour == PieceColour.White
            ? move.To.Column < move.From.Column
            : move.To.Column > move.From.Column;

        var rowToFetch = king.Colour == PieceColour.White ? 7 : 0;
        var colToFetch = isQueenSide ? 0 : 7;

        var rookStartingPosition = new Position(rowToFetch, colToFetch);

        var rookCastling = _board.GetPiece(rookStartingPosition);

        if (rookCastling is not Rook rook) return false;

        if (rook.Colour != king.Colour) return false;

        if (rook.HasMoved) return false;

        var step = isQueenSide ? -1 : 1;

        // determine if path is clear
        for (var col = move.From.Column + step; col != colToFetch + step; col += step)
        {
            var position = new Position(move.From.Row, col);

            if (_board.GetPiece(position) is not null)
                return false;

            if (col != colToFetch && _board.IsSquareAttacked(position, king.Colour.Opposite()))
                return false ;
        }

        return true;
    }
}
using Chess.Core.Pieces;

namespace Chess.Core;

public enum GameStatus { Ongoing, Check, Checkmate, Stalemate }
public class ChessGame(IChessBoard board) : IChessGame
{
    public PieceColour ToMove { get; private set; } = PieceColour.White;
    public GameStatus Status { get; private set; } = GameStatus.Ongoing;
    private readonly IChessBoard _board = board;

    public MoveResult MakeMove(ChessMove move)
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

    private bool IsMoveFullyLegal(ChessMove move, ChessPiece piece)
    {
        if (piece is King king && Math.Abs(move.To.col - move.From.col) == 2)
            return IsCastleValid(move, king);

        return IsKingInCheck(move);
    }

    private bool IsKingInCheck(ChessMove move)
    {
        throw new NotImplementedException();
    }

    private bool IsCastleValid(ChessMove move, King king)
    {
        var rowDiff = Math.Abs(move.To.row - move.From.row);
        var colDiff = Math.Abs(move.To.col - move.From.col);

        var isCastlePattern = rowDiff == 0 && colDiff == 2;

        if (!isCastlePattern)
        {
            return false;
        }

        if (_board.HasKingMoved(move.From, king))
        {
            return false;
        }

        var isQueenSide = king.Colour == PieceColour.White
            ? move.To.col < move.From.col
            : move.To.col > move.From.col;

        if (_board.HasRookMoved(isQueenSide, king.Colour))
        {
            return false;
        }

        return true;
    }
}
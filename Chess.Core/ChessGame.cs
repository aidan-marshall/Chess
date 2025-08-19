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

        if (!piece.ValidMove(move, _board))
            return MoveResult.IllegalMove;

        if (!IsMoveFullyLegal(move))
            return MoveResult.MoveLeavesKingInCheck;

        return MoveResult.Success;
    }

    private bool IsMoveFullyLegal(ChessMove move)
    {
        var piece = _board.GetPiece(move.From);

        if (piece is King && Math.Abs(move.To.col - move.From.col) == 2)
            return IsCastleValid(move);

        return IsKingInCheck(move);
    }

    private bool IsKingInCheck(ChessMove move)
    {
        throw new NotImplementedException();
    }

    private bool IsSquareAttacked((int, int) kingPosition, PieceColour colour)
    {
        throw new NotImplementedException();
    }

    private bool IsCastleValid(ChessMove move)
    {
        throw new NotImplementedException();
    }
}
using Chess.Engine.Moves;
using Chess.Engine.Pieces;

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

    public bool MakeMove(Move move)
    {
        throw new NotImplementedException();
    }
}
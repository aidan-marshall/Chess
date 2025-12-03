namespace Chess.Engine;

internal interface IChessGame
{
    PieceColour ToMove { get; }
    GameStatus Status { get; }
    MoveResult MakeMove(Move move);
}
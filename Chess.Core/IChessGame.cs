namespace Chess.Engine;

internal interface IChessGame
{
    PieceColour ToMove { get; }
    GameStatus Status { get; }
    bool MakeMove(Move move);
}
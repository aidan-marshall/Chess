namespace Chess.Core;

public class ChessMove((int row, int col) from, (int row, int col) to)
{
    public (int row, int col) From { get; } = from;
    public (int row, int col) To { get; } = to;
}
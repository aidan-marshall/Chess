namespace Chess.Core;

public class Move(Position from, Position to)
{
    public Position From { get; } = from;
    public Position To { get; } = to;
}
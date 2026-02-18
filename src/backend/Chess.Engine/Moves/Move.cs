namespace Chess.Engine.Moves;

public readonly record struct Move(Position From, Position To)
{
    public Position From { get; } = From;
    public Position To { get; } = To;
    public int ColDiff => To.Column - From.Column;
    public int RowDiff => To.Row - From.Row;

    public static Move Of(int fromRow, int fromCol, int toRow, int toCol)
        => new
        (
            new Position(fromRow, fromCol),
            new Position(toRow, toCol)
        );

    public static Move Of(Position from, Position to)
    {
        return new Move(from, to);
    }
}
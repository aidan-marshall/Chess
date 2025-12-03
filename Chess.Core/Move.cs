namespace Chess.Engine;

internal readonly record struct Move
{
    public Position From { get; }
    public Position To { get; }
    public int ColDiff => To.Column - From.Column;
    public int RowDiff => To.Row - From.Row;

    internal Move(Position from, Position to)
    {
        From = from;
        To = to;
    }

    public static Move Of(int fromRow, int fromCol, int toRow, int toCol)
        => new
        (
            new Position(fromRow, fromCol),
            new Position(toRow, toCol)
        );
}
namespace Chess.Engine.Moves;

internal readonly record struct Position(int Row, int Column)
{
    public int Row { get; } = Row;
    public int Column { get; } = Column;
    public (int Row, int Column) ToTuple() => (Row, Column);

    public bool IsWithinBounds()
    {
        return Row >= 0 && Row < 8 &&
               Column >= 0 && Column < 8;
    }

    internal static Position Of(int row, int col)
    {
        return new Position(row, col);
    }
}

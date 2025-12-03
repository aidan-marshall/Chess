namespace Chess.Engine;

internal readonly record struct Position(int Row, int column)
{
    public int Row { get; } = Row;
    public int Column { get; } = column;
    public (int Row, int Column) ToTuple() => (Row, Column);

    public bool IsWithinBounds()
    {
        return Row >= 0 && Row < 8 &&
               Column >= 0 && Column < 8;
    }
}

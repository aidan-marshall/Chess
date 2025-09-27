namespace Chess.Core;

public class Position(int row, int column)
{
    public int Row { get; } = row;
    public int Column { get; } = column;
    public (int Row, int Column) ToTuple() => (Row, Column);
}

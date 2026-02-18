namespace Chess.Engine.Pieces;

public sealed class Piece(PieceType type, PieceColour colour, int moveAmount = 0)
{
    public PieceType Type { get; } = type;
    public PieceColour Colour { get; } = colour;
    public int MoveAmount { get; set; } = moveAmount;
    public bool HasMoved => MoveAmount > 0;

    public Piece Clone()
    {
        return new Piece(this.Type, this.Colour, this.MoveAmount);
    }

    public static Piece Pawn(PieceColour colour) => new(PieceType.Pawn, colour);

    public static Piece Rook(PieceColour colour) => new(PieceType.Rook, colour);

    public static Piece Knight(PieceColour colour) => new(PieceType.Knight, colour);

    public static Piece Bishop(PieceColour colour) => new(PieceType.Bishop, colour);

    public static Piece Queen(PieceColour colour) => new(PieceType.Queen, colour);

    public static Piece King(PieceColour colour) => new(PieceType.King, colour);
}

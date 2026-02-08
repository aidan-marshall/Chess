using Chess.Engine.Pieces;

namespace Chess.Application.Dtos;

public sealed record MoveDto
{
    public int Id { get; set; }
    public int MoveNumber { get; set; }
    public PieceColour Colour { get; set; }
    public required string AlgebraicNotation { get; set; }
    public required string FromSquare { get; set; }
    public required string ToSquare { get; set; }
    public PieceType? CapturedPiece { get; set; }
    public PieceType? PromotionPiece { get; set; }
    public bool IsCheck { get; set; }
    public bool IsCheckmate { get; set; }
    public DateTime TimestampUtc { get; set; }
}

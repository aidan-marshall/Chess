using Chess.Engine.Pieces;

namespace Chess.Data.EntityFramework.Entities;

public class Move
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public int MoveNumber { get; set; }
    public PieceColour Colour { get; set; }
    public required string AlgebraicNotation { get; set; }
    public required string FromSquare { get; set; } // e.g. "e2"
    public required string ToSquare { get; set; } // e.g. "e2"
    public PieceType? CapturedPiece { get; set; }
    public PieceType? PromotionPiece { get; set; }
    public bool IsCheck { get; set; }
    public bool IsCheckmate { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Game Game { get; set; } = null!;
}

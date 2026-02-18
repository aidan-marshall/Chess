namespace Chess.Application.Dtos;

public sealed record MakeMoveDto
{
    public required string FromSquare { get; init; }
    public required string ToSquare { get; init; }
    public string? PromotionPiece { get; init; }
}

namespace Chess.Application.Dtos;

public sealed record DrawActionDto
{
    public required string Colour { get; init; }
}

namespace Chess.Application.Dtos;

public sealed record CreateGameDto
{
    public string? FenPosition { get; init; }
}

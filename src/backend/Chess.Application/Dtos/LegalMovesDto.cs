namespace Chess.Application.Dtos;

public sealed record LegalMovesDto
{
    public string FromSquare { get; init; } = null!;
    public List<string> LegalDestinations { get; init; } = [];
}

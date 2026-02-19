using Chess.Engine.Pieces;

namespace Chess.Application.Dtos;

public sealed record ResignDto
{
    public required string Colour { get; init; }
}

using Chess.Engine.Pieces;

namespace Chess.Application.Dtos;

public sealed record DrawActionDto
{
    public required PieceColour Colour { get; init; }
}

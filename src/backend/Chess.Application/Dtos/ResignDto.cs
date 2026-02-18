using Chess.Engine.Pieces;

namespace Chess.Application.Dtos;

public sealed record ResignDto
{
    public required PieceColour Colour { get; init; }
}

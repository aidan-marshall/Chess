namespace Chess.Application.Dtos;

public sealed record GameDto
{
    public int Id { get; init; }
    public string FenPosition { get; init; } = null!;
    public string GameState { get; init; } = null!;
    public string? GameResult { get; init; }
    public string? DrawReason { get; init; }
    public string ToMove { get; init; } = null!;
    public int FullMoveNumber { get; init; }
    public int HalfMoveClock { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public List<MoveDto> Moves { get; set; } = [];
}

using Chess.Engine.Game;
using Chess.Engine.Pieces;

namespace Chess.Data.EntityFramework.Entities;

public class Game
{
    public int Id { get; set; }
    public required string FenPosition { get; set; }
    public GameState GameState { get; set; }
    public GameResult? GameResult { get; set; }
    public DrawReason? DrawReason { get; set; }
    public PieceColour ToMove { get; set; }
    public int FullMoveNumber { get; set; }
    public int HalfMoveClock { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public ICollection<Move> Moves { get; set; } = [];
}

using Chess.Application.Dtos;
using Chess.Data.EntityFramework.Entities;

namespace Chess.Application.Mappers;

public static class GameMapper
{
    /// <summary>
    /// Converts a GameEntity to a GameDto.
    /// Includes all game properties and move history.
    /// </summary>
    /// <param name="entity">The game entity from the database</param>
    /// <returns>Game DTO for API response</returns>
    public static GameDto ToDto(Game entity)
    {
        return new GameDto
        {
            Id = entity.Id,
            FenPosition = entity.FenPosition,
            GameState = entity.GameState.ToString(),
            GameResult = entity.GameResult.ToString(),
            DrawReason = entity.DrawReason.ToString(),
            ToMove = entity.ToMove.ToString(),
            FullMoveNumber = entity.FullMoveNumber,
            HalfMoveClock = entity.HalfMoveClock,
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc,
            Moves = [.. entity.Moves
                .OrderBy(m => m.CreatedAtUtc)
                .Select(MoveMapper.ToDto)]
        };
    }

    /// <summary>
    /// Creates a list of GameDto from multiple GameEntity objects.
    /// Useful for listing multiple games.
    /// </summary>
    /// <param name="entities">Collection of game entities</param>
    /// <returns>List of game DTOs</returns>
    public static List<GameDto> ToDtoList(IEnumerable<Game> entities)
    {
        return [.. entities.Select(ToDto)];
    }
}

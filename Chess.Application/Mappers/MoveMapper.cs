using Chess.Application.Dtos;
using Chess.Data.EntityFramework.Entities;

namespace Chess.Application.Mappers;

public static class MoveMapper
{
    /// <summary>
    /// Converts a MoveEntity to a MoveDto.
    /// </summary>
    /// <param name="entity">The move entity from the database</param>
    /// <returns>Move DTO for API response</returns>
    public static MoveDto ToDto(Move entity)
    {
        return new MoveDto
        {
            Id = entity.Id,
            MoveNumber = entity.MoveNumber,
            Colour = entity.Colour,
            AlgebraicNotation = entity.AlgebraicNotation,
            FromSquare = entity.FromSquare,
            ToSquare = entity.ToSquare,
            CapturedPiece = entity.CapturedPiece,
            PromotionPiece = entity.PromotionPiece,
            IsCheck = entity.IsCheck,
            IsCheckmate = entity.IsCheckmate,
            TimestampUtc = entity.CreatedAtUtc
        };
    }

    /// <summary>
    /// Creates a list of MoveDto from multiple MoveEntity objects.
    /// </summary>
    /// <param name="entities">Collection of move entities</param>
    /// <returns>List of move DTOs</returns>
    public static List<MoveDto> ToDtoList(IEnumerable<Move> entities)
    {
        return [.. entities.Select(ToDto)];
    }
}

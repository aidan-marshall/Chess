using Chess.Application.Dtos;

namespace Chess.Application.Services;

internal interface IGameService
{
    Task<GameDto?> CreateGameAsync(CreateGameDto request);
    Task<GameDto?> GetGameAsync(int gameId);
    Task<GameDto?> MakeMoveAsync(int gameId, MakeMoveDto move);
}

using Chess.Application.Dtos;

namespace Chess.Application.Services;

public interface IGameService
{
    Task<GameDto?> CreateGameAsync(CreateGameDto request);
    Task<GameDto?> GetGameAsync(int gameId);
    Task<GameDto?> MakeMoveAsync(int gameId, MakeMoveDto move);
    Task<LegalMovesDto?> GetLegalMovesAsync(int gameId, string fromSquare);
    Task<GameDto?> ResignAsync(int gameId, ResignDto request);
    Task<GameDto?> OfferDrawAsync(int gameId, DrawActionDto request);
    Task<GameDto?> AcceptDrawAsync(int gameId, DrawActionDto request);
    Task<GameDto?> DeclineDrawAsync(int gameId, DrawActionDto request);
}

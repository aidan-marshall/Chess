using Chess.Application.Dtos;
using Chess.Engine.Pieces;

namespace Chess.Application.Services;

public interface IGameService
{
    Task<GameDto> CreateGameAsync(string? fenPosition);
    Task<GameDto?> GetGameAsync(int gameId);
    Task<GameDto?> MakeMoveAsync(int gameId, MakeMoveDto move);
    Task<LegalMovesDto?> GetLegalMovesAsync(int gameId, string fromSquare);
    Task<GameDto?> ResignAsync(int gameId, PieceColour pieceColour);
    Task<GameDto?> OfferDrawAsync(int gameId, PieceColour pieceColour);
    Task<GameDto?> AcceptDrawAsync(int gameId, PieceColour pieceColour);
    Task<GameDto?> DeclineDrawAsync(int gameId);
}

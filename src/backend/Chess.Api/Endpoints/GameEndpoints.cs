using Chess.Application.Dtos;
using Chess.Application.Services;
using Chess.Engine.Pieces;

namespace Chess.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/games");

        group.MapPost("/", CreateGame);
        group.MapGet("/{id:int}", GetGame).WithName("GetGameById");
        group.MapPost("/{id:int}/moves", MakeMove);
        group.MapGet("/{id:int}/legal-moves/{from}", GetLegalMoves);
        group.MapPost("/{id:int}/resign", Resign);
        group.MapPost("/{id:int}/draw/offer", OfferDraw);
        group.MapPost("/{id:int}/draw/accept", AcceptDraw);
        group.MapPost("/{id:int}/draw/decline", DeclineDraw);
    }

    private static async Task<IResult> CreateGame(CreateGameDto dto, IGameService svc)
    {
        try
        {
            var game = await svc.CreateGameAsync(dto.FenPosition);
            
            return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, game);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> GetGame(int id, IGameService svc)
    {
        var game = await svc.GetGameAsync(id);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }

    private static async Task<IResult> MakeMove(int id, MakeMoveDto dto, IGameService svc)
    {
        try
        {
            var game = await svc.MakeMoveAsync(id, dto);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }

    private static async Task<IResult> GetLegalMoves(int id, string from, IGameService svc)
    {
        var result = await svc.GetLegalMovesAsync(id, from);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> Resign(int id, ResignDto dto, IGameService svc)
    {
        try
        {
            var pieceColour = TryParsePieceColour(dto.Colour);

            if (!pieceColour.HasValue)
                return Results.BadRequest(new { error = "Invalid colour. Must be 'White' or 'Black'" });

            var game = await svc.ResignAsync(id, pieceColour.Value);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }

    private static async Task<IResult> OfferDraw(int id, DrawActionDto dto, IGameService svc)
    {
        try
        {
            var pieceColour = TryParsePieceColour(dto.Colour);

            if (!pieceColour.HasValue)
                return Results.BadRequest(new { error = "Invalid colour. Must be 'White' or 'Black'" });

            var game = await svc.OfferDrawAsync(id, pieceColour.Value);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }

    private static async Task<IResult> AcceptDraw(int id, DrawActionDto dto, IGameService svc)
    {
        try
        {
            var pieceColour = TryParsePieceColour(dto.Colour);

            if (!pieceColour.HasValue)
                return Results.BadRequest(new { error = "Invalid colour. Must be 'White' or 'Black'" });

            var game = await svc.AcceptDrawAsync(id, pieceColour.Value);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }

    private static async Task<IResult> DeclineDraw(int id, IGameService svc)
    {
        try
        {
            var game = await svc.DeclineDrawAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }

    private static PieceColour? TryParsePieceColour(string pieceColourStr)
    {
        var result = Enum.TryParse<PieceColour>(pieceColourStr, ignoreCase: true, out var pieceColour);

        return result ? pieceColour : null;
    }
}

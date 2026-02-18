using Chess.Application.Dtos;
using Chess.Application.Services;
using Chess.Data.EntityFramework;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChessDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ChessDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseCors();

const string CurrentVersion = "v1";

var games = app.MapGroup($"/api/{CurrentVersion}/games");

games.MapPost("/", async (CreateGameDto dto, IGameService svc) =>
{
    try
    {
        var game = await svc.CreateGameAsync(dto);
        return Results.Created($"/api/{CurrentVersion}/games/{game!.Id}", game);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

games.MapGet("/{id:int}", async (int id, IGameService svc) =>
{
    var game = await svc.GetGameAsync(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

games.MapPost("/{id:int}/moves", async (int id, MakeMoveDto dto, IGameService svc) =>
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
});

games.MapGet("/{id:int}/legal-moves/{from}", async (int id, string from, IGameService svc) =>
{
    var result = await svc.GetLegalMovesAsync(id, from);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

games.MapPost("/{id:int}/resign", async (int id, ResignDto dto, IGameService svc) =>
{
    try
    {
        var game = await svc.ResignAsync(id, dto);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

games.MapPost("/{id:int}/draw/offer", async (int id, DrawActionDto dto, IGameService svc) =>
{
    try
    {
        var game = await svc.OfferDrawAsync(id, dto);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

games.MapPost("/{id:int}/draw/accept", async (int id, DrawActionDto dto, IGameService svc) =>
{
    try
    {
        var game = await svc.AcceptDrawAsync(id, dto);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

games.MapPost("/{id:int}/draw/decline", async (int id, DrawActionDto dto, IGameService svc) =>
{ try
    {
        var game = await svc.DeclineDrawAsync(id, dto);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

app.Run();

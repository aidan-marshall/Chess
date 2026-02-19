using Chess.Api.Endpoints;
using Chess.Application.Services;
using Chess.Data.EntityFramework;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;

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
app.UseExceptionHandler(err => err.Run(async ctx =>
{
    var logger = ctx.RequestServices.GetRequiredService<ILogger<Program>>();
    var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;

    logger.LogError(ex, "Unhandled exception occurred");

    ctx.Response.StatusCode = 500;
    await ctx.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });
}));

const string currentVersion = "v1";

var api = app.MapGroup($"/api/{currentVersion}");
api.MapGameEndpoints();

app.Run();

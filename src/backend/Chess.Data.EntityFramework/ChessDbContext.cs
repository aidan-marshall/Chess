using Chess.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chess.Data.EntityFramework;

public class ChessDbContext(DbContextOptions<ChessDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Move> Moves { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChessDbContext).Assembly);
    }

}

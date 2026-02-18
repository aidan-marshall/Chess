using Chess.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chess.Data.EntityFramework.Configurations;

/// <summary>
/// Entity Framework configuration for the Game entity.
/// </summary>
public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        // Primary key
        builder.HasKey(g => g.Id);

        // Properties
        builder.Property(g => g.FenPosition)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Current board position in FEN notation");

        builder.Property(g => g.GameState)
            .IsRequired()
            .HasConversion<string>() // Store as string in database
            .HasMaxLength(20);

        builder.Property(g => g.GameResult)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(g => g.DrawReason)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(g => g.ToMove)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(g => g.FullMoveNumber)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(g => g.HalfMoveClock)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(g => g.CreatedAtUtc)
            .IsRequired();

        builder.Property(g => g.UpdatedAtUtc)
            .IsRequired();

        // Indexes
        builder.HasIndex(g => g.GameState);

        builder.HasIndex(g => g.CreatedAtUtc);

        // Relationships
        builder.HasMany(g => g.Moves)
            .WithOne(m => m.Game)
            .HasForeignKey(m => m.GameId)
            .OnDelete(DeleteBehavior.Cascade); // Delete moves when game is deleted
    }
}

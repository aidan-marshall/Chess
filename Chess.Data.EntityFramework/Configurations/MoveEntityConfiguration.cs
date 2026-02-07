using Chess.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chess.Data.EntityFramework.Configurations;

/// <summary>
/// Entity Framework configuration for the Move entity.
/// </summary>
public class MoveEntityConfiguration : IEntityTypeConfiguration<Move>
{
    public void Configure(EntityTypeBuilder<Move> builder)
    {
        // Table name
        builder.ToTable("Moves");

        // Primary key
        builder.HasKey(m => m.Id);

        // Properties
        builder.Property(m => m.GameId)
            .IsRequired();

        builder.Property(m => m.MoveNumber)
            .IsRequired();

        builder.Property(m => m.Colour)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(m => m.AlgebraicNotation)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("Move in algebraic notation (e.g., 'e4', 'Nf3', 'O-O')");

        builder.Property(m => m.FromSquare)
            .IsRequired()
            .HasMaxLength(2)
            .HasComment("Starting square (e.g., 'e2')");

        builder.Property(m => m.ToSquare)
            .IsRequired()
            .HasMaxLength(2)
            .HasComment("Destination square (e.g., 'e4')");

        builder.Property(m => m.CapturedPiece)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(m => m.PromotionPiece)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(m => m.IsCheck)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.IsCheckmate)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAtUtc)
            .IsRequired();

        // Indexes
        builder.HasIndex(m => m.GameId);

        builder.HasIndex(m => new { m.GameId, m.MoveNumber })
            .IsUnique(); // Each game can only have one move per move number

        builder.HasIndex(m => m.CreatedAtUtc);

        // Relationships
        builder.HasOne(m => m.Game)
            .WithMany(g => g.Moves)
            .HasForeignKey(m => m.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
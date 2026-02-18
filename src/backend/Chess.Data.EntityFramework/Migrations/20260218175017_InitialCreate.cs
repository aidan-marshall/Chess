using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chess.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FenPosition = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Current board position in FEN notation"),
                    GameState = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GameResult = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    DrawReason = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ToMove = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FullMoveNumber = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    HalfMoveClock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DrawOfferedBy = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    MoveNumber = table.Column<int>(type: "integer", nullable: false),
                    Colour = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AlgebraicNotation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, comment: "Move in algebraic notation (e.g., 'e4', 'Nf3', 'O-O')"),
                    FromSquare = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, comment: "Starting square (e.g., 'e2')"),
                    ToSquare = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, comment: "Destination square (e.g., 'e4')"),
                    CapturedPiece = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PromotionPiece = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IsCheck = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsCheckmate = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moves_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatedAtUtc",
                table: "Games",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameState",
                table: "Games",
                column: "GameState");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_CreatedAtUtc",
                table: "Moves",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameId",
                table: "Moves",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameId_MoveNumber",
                table: "Moves",
                columns: new[] { "GameId", "MoveNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}

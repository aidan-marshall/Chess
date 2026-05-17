using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class FixMoveNumberUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Moves_GameId_MoveNumber",
                table: "Moves");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameId_MoveNumber_Colour",
                table: "Moves",
                columns: new[] { "GameId", "MoveNumber", "Colour" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Moves_GameId_MoveNumber_Colour",
                table: "Moves");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameId_MoveNumber",
                table: "Moves",
                columns: new[] { "GameId", "MoveNumber" },
                unique: true);
        }
    }
}

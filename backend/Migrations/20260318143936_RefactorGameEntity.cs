using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RefactorGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameState_Cards_CardId",
                table: "GameState");

            migrationBuilder.DropForeignKey(
                name: "FK_GameState_Games_GameId",
                table: "GameState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameState",
                table: "GameState");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35be9970-3745-417f-bd68-087c7c4b838f");

            migrationBuilder.RenameTable(
                name: "GameState",
                newName: "GameStates");

            migrationBuilder.RenameIndex(
                name: "IX_GameState_GameId",
                table: "GameStates",
                newName: "IX_GameStates_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameState_CardId",
                table: "GameStates",
                newName: "IX_GameStates_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "368b9499-beaf-4856-96c7-9bbd07414abf", null, "User", "USER" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Cards_CardId",
                table: "GameStates",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Cards_CardId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "368b9499-beaf-4856-96c7-9bbd07414abf");

            migrationBuilder.RenameTable(
                name: "GameStates",
                newName: "GameState");

            migrationBuilder.RenameIndex(
                name: "IX_GameStates_GameId",
                table: "GameState",
                newName: "IX_GameState_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameStates_CardId",
                table: "GameState",
                newName: "IX_GameState_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameState",
                table: "GameState",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35be9970-3745-417f-bd68-087c7c4b838f", null, "User", "USER" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameState_Cards_CardId",
                table: "GameState",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameState_Games_GameId",
                table: "GameState",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

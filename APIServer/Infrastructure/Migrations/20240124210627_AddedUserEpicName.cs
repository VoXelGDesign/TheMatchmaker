using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserEpicName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EpicName",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Player1_EpicName",
                table: "RocketLeague3vs3Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Player2_EpicName",
                table: "RocketLeague3vs3Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Player3_EpicName",
                table: "RocketLeague3vs3Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Player1_EpicName",
                table: "RocketLeague2vs2Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Player2_EpicName",
                table: "RocketLeague2vs2Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpicName",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Player1_EpicName",
                table: "RocketLeague3vs3Lobbies");

            migrationBuilder.DropColumn(
                name: "Player2_EpicName",
                table: "RocketLeague3vs3Lobbies");

            migrationBuilder.DropColumn(
                name: "Player3_EpicName",
                table: "RocketLeague3vs3Lobbies");

            migrationBuilder.DropColumn(
                name: "Player1_EpicName",
                table: "RocketLeague2vs2Lobbies");

            migrationBuilder.DropColumn(
                name: "Player2_EpicName",
                table: "RocketLeague2vs2Lobbies");
        }
    }
}

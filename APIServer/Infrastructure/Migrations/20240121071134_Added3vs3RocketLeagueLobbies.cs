using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added3vs3RocketLeagueLobbies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RocketLeague3vs3Lobbies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Player1_UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Player1_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player1_SteamProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player1_DiscordName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player1_IsReady = table.Column<bool>(type: "bit", nullable: false),
                    Player2_UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Player2_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player2_SteamProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player2_DiscordName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player2_IsReady = table.Column<bool>(type: "bit", nullable: false),
                    Player3_UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Player3_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player3_SteamProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player3_DiscordName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player3_IsReady = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RocketLeague3vs3Lobbies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RocketLeague3vs3Lobbies");
        }
    }
}

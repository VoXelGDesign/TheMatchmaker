using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added2v2RocketLeagueLobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RocketLeague2vs2Lobbies",
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RocketLeague2vs2Lobbies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RocketLeague2vs2Lobbies");
        }
    }
}

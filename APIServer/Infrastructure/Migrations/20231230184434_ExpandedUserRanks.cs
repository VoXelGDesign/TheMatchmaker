using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandedUserRanks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RocketLeagueRankNumber",
                table: "UserGameRanks",
                newName: "TwoVsTwo_RocketLeagueRankNumber");

            migrationBuilder.RenameColumn(
                name: "RocketLeagueRankName",
                table: "UserGameRanks",
                newName: "TwoVsTwo_RocketLeagueRankName");

            migrationBuilder.RenameColumn(
                name: "RocketLeagueDivision",
                table: "UserGameRanks",
                newName: "TwoVsTwo_RocketLeagueDivision");

            migrationBuilder.AddColumn<string>(
                name: "ThreeVsThreeRocketLeagueDivision",
                table: "UserGameRanks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreeVsThreeRocketLeagueRankName",
                table: "UserGameRanks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreeVsThreeRocketLeagueRankNumber",
                table: "UserGameRanks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreeVsThreeRocketLeagueDivision",
                table: "UserGameRanks");

            migrationBuilder.DropColumn(
                name: "ThreeVsThreeRocketLeagueRankName",
                table: "UserGameRanks");

            migrationBuilder.DropColumn(
                name: "ThreeVsThreeRocketLeagueRankNumber",
                table: "UserGameRanks");

            migrationBuilder.RenameColumn(
                name: "TwoVsTwo_RocketLeagueRankNumber",
                table: "UserGameRanks",
                newName: "RocketLeagueRankNumber");

            migrationBuilder.RenameColumn(
                name: "TwoVsTwo_RocketLeagueRankName",
                table: "UserGameRanks",
                newName: "RocketLeagueRankName");

            migrationBuilder.RenameColumn(
                name: "TwoVsTwo_RocketLeagueDivision",
                table: "UserGameRanks",
                newName: "RocketLeagueDivision");
        }
    }
}

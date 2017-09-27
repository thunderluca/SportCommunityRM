using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportCommunityRM.Data.Migrations
{
    public partial class Initial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamScore",
                table: "SCRM_Activities",
                newName: "Score");

            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "SCRM_Contents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SCRM_MatchScore",
                columns: table => new
                {
                    RegisteredUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_MatchScore", x => new { x.RegisteredUserId, x.MatchId });
                    table.ForeignKey(
                        name: "FK_SCRM_MatchScore_SCRM_Activities_MatchId",
                        column: x => x.MatchId,
                        principalTable: "SCRM_Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SCRM_MatchScore_SCRM_RegisteredUsers_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "SCRM_RegisteredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_MatchScore_MatchId",
                table: "SCRM_MatchScore",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCRM_MatchScore");

            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "SCRM_Contents");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "SCRM_Activities",
                newName: "TeamScore");
        }
    }
}

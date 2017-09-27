using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportCommunityRM.Data.Migrations
{
    public partial class Initial9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SCRM_Media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCRM_Media_SCRM_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "SCRM_Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_RegisteredUserMediaTag",
                columns: table => new
                {
                    RegisteredUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_RegisteredUserMediaTag", x => new { x.RegisteredUserId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_SCRM_RegisteredUserMediaTag_SCRM_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "SCRM_Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SCRM_RegisteredUserMediaTag_SCRM_RegisteredUsers_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "SCRM_RegisteredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Media_ActivityId",
                table: "SCRM_Media",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_RegisteredUserMediaTag_MediaId",
                table: "SCRM_RegisteredUserMediaTag",
                column: "MediaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCRM_RegisteredUserMediaTag");

            migrationBuilder.DropTable(
                name: "SCRM_Media");
        }
    }
}

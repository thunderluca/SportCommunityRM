using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportCommunityRM.Data.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SCRM_Coach_SCRM_RegisteredUsers_RegisteredUserId",
                table: "SCRM_Coach");

            migrationBuilder.DropForeignKey(
                name: "FK_SCRM_TeamCoach_SCRM_Coach_CoachId",
                table: "SCRM_TeamCoach");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SCRM_Coach",
                table: "SCRM_Coach");

            migrationBuilder.RenameTable(
                name: "SCRM_Coach",
                newName: "SCRM_Coaches");

            migrationBuilder.RenameIndex(
                name: "IX_SCRM_Coach_RegisteredUserId",
                table: "SCRM_Coaches",
                newName: "IX_SCRM_Coaches_RegisteredUserId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "SCRM_RegisteredUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "SCRM_RegisteredUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CivicNumber",
                table: "SCRM_RegisteredUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "SCRM_RegisteredUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "SCRM_RegisteredUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SCRM_Coaches",
                table: "SCRM_Coaches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SCRM_Coaches_SCRM_RegisteredUsers_RegisteredUserId",
                table: "SCRM_Coaches",
                column: "RegisteredUserId",
                principalTable: "SCRM_RegisteredUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SCRM_TeamCoach_SCRM_Coaches_CoachId",
                table: "SCRM_TeamCoach",
                column: "CoachId",
                principalTable: "SCRM_Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SCRM_Coaches_SCRM_RegisteredUsers_RegisteredUserId",
                table: "SCRM_Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_SCRM_TeamCoach_SCRM_Coaches_CoachId",
                table: "SCRM_TeamCoach");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SCRM_Coaches",
                table: "SCRM_Coaches");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "SCRM_RegisteredUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "SCRM_RegisteredUsers");

            migrationBuilder.DropColumn(
                name: "CivicNumber",
                table: "SCRM_RegisteredUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "SCRM_RegisteredUsers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "SCRM_RegisteredUsers");

            migrationBuilder.RenameTable(
                name: "SCRM_Coaches",
                newName: "SCRM_Coach");

            migrationBuilder.RenameIndex(
                name: "IX_SCRM_Coaches_RegisteredUserId",
                table: "SCRM_Coach",
                newName: "IX_SCRM_Coach_RegisteredUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SCRM_Coach",
                table: "SCRM_Coach",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SCRM_Coach_SCRM_RegisteredUsers_RegisteredUserId",
                table: "SCRM_Coach",
                column: "RegisteredUserId",
                principalTable: "SCRM_RegisteredUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SCRM_TeamCoach_SCRM_Coach_CoachId",
                table: "SCRM_TeamCoach",
                column: "CoachId",
                principalTable: "SCRM_Coach",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

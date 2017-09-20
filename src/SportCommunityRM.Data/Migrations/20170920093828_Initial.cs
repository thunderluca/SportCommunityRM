using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportCommunityRM.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SCRM_Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CivicNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_RegisteredUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_RegisteredUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxBirthYear = table.Column<int>(type: "int", nullable: true),
                    MinBirthYear = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_Fields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCRM_Fields_SCRM_Addresses_LocationId",
                        column: x => x.LocationId,
                        principalTable: "SCRM_Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_Coach",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisteredUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Coach", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCRM_Coach_SCRM_RegisteredUsers_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "SCRM_RegisteredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_Inscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountDue = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartialAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Inscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCRM_Inscriptions_SCRM_RegisteredUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SCRM_RegisteredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_MedicalCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_MedicalCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCRM_MedicalCertificates_SCRM_RegisteredUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SCRM_RegisteredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_RegisteredUserTeam",
                columns: table => new
                {
                    RegisteredUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_RegisteredUserTeam", x => new { x.RegisteredUserId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_SCRM_RegisteredUserTeam_SCRM_RegisteredUsers_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "SCRM_RegisteredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SCRM_RegisteredUserTeam_SCRM_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "SCRM_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnemyTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnemyTeamScore = table.Column<int>(type: "int", nullable: true),
                    TeamScore = table.Column<int>(type: "int", nullable: true),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCRM_Activities_SCRM_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "SCRM_Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SCRM_Activities_SCRM_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "SCRM_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SCRM_Activities_SCRM_Activities_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "SCRM_Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SCRM_TeamCoach",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRM_TeamCoach", x => new { x.TeamId, x.CoachId });
                    table.ForeignKey(
                        name: "FK_SCRM_TeamCoach_SCRM_Coach_CoachId",
                        column: x => x.CoachId,
                        principalTable: "SCRM_Coach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SCRM_TeamCoach_SCRM_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "SCRM_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Activities_FieldId",
                table: "SCRM_Activities",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Activities_TeamId",
                table: "SCRM_Activities",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Activities_TournamentId",
                table: "SCRM_Activities",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Coach_RegisteredUserId",
                table: "SCRM_Coach",
                column: "RegisteredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Fields_LocationId",
                table: "SCRM_Fields",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_Inscriptions_UserId",
                table: "SCRM_Inscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_MedicalCertificates_UserId",
                table: "SCRM_MedicalCertificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_RegisteredUserTeam_TeamId",
                table: "SCRM_RegisteredUserTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_SCRM_TeamCoach_CoachId",
                table: "SCRM_TeamCoach",
                column: "CoachId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCRM_Activities");

            migrationBuilder.DropTable(
                name: "SCRM_Inscriptions");

            migrationBuilder.DropTable(
                name: "SCRM_MedicalCertificates");

            migrationBuilder.DropTable(
                name: "SCRM_RegisteredUserTeam");

            migrationBuilder.DropTable(
                name: "SCRM_TeamCoach");

            migrationBuilder.DropTable(
                name: "SCRM_Fields");

            migrationBuilder.DropTable(
                name: "SCRM_Coach");

            migrationBuilder.DropTable(
                name: "SCRM_Teams");

            migrationBuilder.DropTable(
                name: "SCRM_Addresses");

            migrationBuilder.DropTable(
                name: "SCRM_RegisteredUsers");
        }
    }
}

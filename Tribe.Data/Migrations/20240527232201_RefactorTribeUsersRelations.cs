using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTribeUsersRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TribeParticipant");

            migrationBuilder.CreateTable(
                name: "TribeParticipants",
                columns: table => new
                {
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TribesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TribeParticipants", x => new { x.ParticipantsId, x.TribesId });
                    table.ForeignKey(
                        name: "FK_TribeParticipants_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TribeParticipants_Tribes_TribesId",
                        column: x => x.TribesId,
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TribeParticipants_TribesId",
                table: "TribeParticipants",
                column: "TribesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TribeParticipants");

            migrationBuilder.CreateTable(
                name: "TribeParticipant",
                columns: table => new
                {
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TribeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TribeParticipant", x => new { x.ApplicationUserId, x.TribeId });
                    table.ForeignKey(
                        name: "FK_TribeParticipant_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TribeParticipant_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TribeParticipant_TribeId",
                table: "TribeParticipant",
                column: "TribeId");
        }
    }
}

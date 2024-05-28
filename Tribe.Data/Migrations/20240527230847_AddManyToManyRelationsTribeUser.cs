using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRelationsTribeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tribes_TribeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tribes_AspNetUsers_CreatorId",
                table: "Tribes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TribeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TribeId",
                table: "AspNetUsers");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tribes_AspNetUsers_CreatorId",
                table: "Tribes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tribes_AspNetUsers_CreatorId",
                table: "Tribes");

            migrationBuilder.DropTable(
                name: "TribeParticipant");

            migrationBuilder.AddColumn<Guid>(
                name: "TribeId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TribeId",
                table: "AspNetUsers",
                column: "TribeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tribes_TribeId",
                table: "AspNetUsers",
                column: "TribeId",
                principalTable: "Tribes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tribes_AspNetUsers_CreatorId",
                table: "Tribes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

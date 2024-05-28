using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTribeCreatorField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Tribes",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Tribes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "TribeId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tribes_CreatorId",
                table: "Tribes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TribeId",
                table: "Tasks",
                column: "TribeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tribes_TribeId",
                table: "Tasks",
                column: "TribeId",
                principalTable: "Tribes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tribes_AspNetUsers_CreatorId",
                table: "Tribes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tribes_TribeId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tribes_AspNetUsers_CreatorId",
                table: "Tribes");

            migrationBuilder.DropIndex(
                name: "IX_Tribes_CreatorId",
                table: "Tribes");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TribeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Tribes");

            migrationBuilder.DropColumn(
                name: "TribeId",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Tribes",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}

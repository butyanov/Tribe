using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class RevertAddTribeFkTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tribes_TribeId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tribes_TribeId",
                table: "Tasks",
                column: "TribeId",
                principalTable: "Tribes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tribes_TribeId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tribes_TribeId",
                table: "Tasks",
                column: "TribeId",
                principalTable: "Tribes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

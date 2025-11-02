using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Dates_DateId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Dates_DateId",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Lists_DateId",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Items_DateId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "Lists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lists_DateId",
                table: "Lists",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DateId",
                table: "Items",
                column: "DateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Dates_DateId",
                table: "Items",
                column: "DateId",
                principalTable: "Dates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Dates_DateId",
                table: "Lists",
                column: "DateId",
                principalTable: "Dates",
                principalColumn: "Id");
        }
    }
}

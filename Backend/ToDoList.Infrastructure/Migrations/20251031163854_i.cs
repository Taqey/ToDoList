using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDates_Dates_DateId",
                table: "ItemDates");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDates_Items_ItemId",
                table: "ItemDates");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemDates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DateId",
                table: "ItemDates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDates_Dates_DateId",
                table: "ItemDates",
                column: "DateId",
                principalTable: "Dates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDates_Items_ItemId",
                table: "ItemDates",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDates_Dates_DateId",
                table: "ItemDates");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDates_Items_ItemId",
                table: "ItemDates");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemDates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DateId",
                table: "ItemDates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDates_Dates_DateId",
                table: "ItemDates",
                column: "DateId",
                principalTable: "Dates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDates_Items_ItemId",
                table: "ItemDates",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

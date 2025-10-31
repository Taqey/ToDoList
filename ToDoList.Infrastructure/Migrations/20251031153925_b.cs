using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Dates_ItemId",
                table: "Lists");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Items_DateId",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Lists_DateId",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Lists_ItemId",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Lists");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Lists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DateItem",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false),
                    datesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateItem", x => new { x.ItemsId, x.datesId });
                    table.ForeignKey(
                        name: "FK_DateItem_Dates_datesId",
                        column: x => x.datesId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DateItem_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DateList",
                columns: table => new
                {
                    ListsId = table.Column<int>(type: "int", nullable: false),
                    datesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateList", x => new { x.ListsId, x.datesId });
                    table.ForeignKey(
                        name: "FK_DateList_Dates_datesId",
                        column: x => x.datesId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DateList_Lists_ListsId",
                        column: x => x.ListsId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ListId",
                table: "Items",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_DateItem_datesId",
                table: "DateItem",
                column: "datesId");

            migrationBuilder.CreateIndex(
                name: "IX_DateList_datesId",
                table: "DateList",
                column: "datesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Lists_ListId",
                table: "Items",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Lists_ListId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "DateItem");

            migrationBuilder.DropTable(
                name: "DateList");

            migrationBuilder.DropIndex(
                name: "IX_Items_ListId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "Lists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Lists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lists_DateId",
                table: "Lists",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_ItemId",
                table: "Lists",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Dates_ItemId",
                table: "Lists",
                column: "ItemId",
                principalTable: "Dates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Items_DateId",
                table: "Lists",
                column: "DateId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

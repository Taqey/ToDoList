using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class c : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateItem");

            migrationBuilder.DropTable(
                name: "DateList");

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

            migrationBuilder.CreateTable(
                name: "ItemDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDates_Dates_DateId",
                        column: x => x.DateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemDates_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListDates_Dates_DateId",
                        column: x => x.DateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListDates_Lists_ListId",
                        column: x => x.ListId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lists_DateId",
                table: "Lists",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DateId",
                table: "Items",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDates_DateId",
                table: "ItemDates",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDates_ItemId",
                table: "ItemDates",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ListDates_DateId",
                table: "ListDates",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_ListDates_ListId",
                table: "ListDates",
                column: "ListId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Dates_DateId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Dates_DateId",
                table: "Lists");

            migrationBuilder.DropTable(
                name: "ItemDates");

            migrationBuilder.DropTable(
                name: "ListDates");

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
                name: "IX_DateItem_datesId",
                table: "DateItem",
                column: "datesId");

            migrationBuilder.CreateIndex(
                name: "IX_DateList_datesId",
                table: "DateList",
                column: "datesId");
        }
    }
}

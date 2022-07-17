using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace everest_app.Migrations
{
    public partial class AddTagToToDoItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagToDoItem",
                columns: table => new
                {
                    TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToDoItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagToDoItem", x => new { x.TagsId, x.ToDoItemsId });
                    table.ForeignKey(
                        name: "FK_TagToDoItem_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TagToDoItem_ToDoItems_ToDoItemsId",
                        column: x => x.ToDoItemsId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagToDoItem_ToDoItemsId",
                table: "TagToDoItem",
                column: "ToDoItemsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagToDoItem");
        }
    }
}

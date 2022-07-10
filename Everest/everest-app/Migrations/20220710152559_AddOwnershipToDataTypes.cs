using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace everest_app.Migrations
{
    public partial class AddOwnershipToDataTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ToDoItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_OwnerId",
                table: "ToDoItems",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_OwnerId",
                table: "Notes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_AspNetUsers_OwnerId",
                table: "ToDoItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_OwnerId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_AspNetUsers_OwnerId",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_OwnerId",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Notes");
        }
    }
}

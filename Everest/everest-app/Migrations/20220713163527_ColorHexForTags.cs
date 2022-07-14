using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace everest_app.Migrations
{
    public partial class ColorHexForTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHexadecimal",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHexadecimal",
                table: "Tags");
        }
    }
}

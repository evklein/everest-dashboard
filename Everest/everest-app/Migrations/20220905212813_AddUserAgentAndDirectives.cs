using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace everest_app.Migrations
{
    public partial class AddUserAgentAndDirectives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserAgentDirectiveId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserAgents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastConnectionActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EverestPrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EverestPublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgentPublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAgents_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAgentDirectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondsToRun = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgentDirectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAgentDirectives_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAgentDirectives_UserAgents_UserAgentId",
                        column: x => x.UserAgentId,
                        principalTable: "UserAgents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UserAgentDirectiveId",
                table: "Tags",
                column: "UserAgentDirectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgentDirectives_OwnerId",
                table: "UserAgentDirectives",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgentDirectives_UserAgentId",
                table: "UserAgentDirectives",
                column: "UserAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgents_OwnerId",
                table: "UserAgents",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_UserAgentDirectives_UserAgentDirectiveId",
                table: "Tags",
                column: "UserAgentDirectiveId",
                principalTable: "UserAgentDirectives",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_UserAgentDirectives_UserAgentDirectiveId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "UserAgentDirectives");

            migrationBuilder.DropTable(
                name: "UserAgents");

            migrationBuilder.DropIndex(
                name: "IX_Tags_UserAgentDirectiveId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UserAgentDirectiveId",
                table: "Tags");
        }
    }
}

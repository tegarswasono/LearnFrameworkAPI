using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFrameworkApi.Module.Migrations
{
    /// <inheritdoc />
    public partial class addRoleFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleFunction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFunction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleFunction_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_OrderIndex",
                table: "Menus",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFunction_RoleId",
                table: "RoleFunction",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleFunction");

            migrationBuilder.DropIndex(
                name: "IX_Menus_OrderIndex",
                table: "Menus");
        }
    }
}

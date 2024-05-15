using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFrameworkApi.Module.Migrations
{
    /// <inheritdoc />
    public partial class roleFunctionAddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "RoleFunctions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFunctions_FunctionId",
                table: "RoleFunctions",
                column: "FunctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoleFunctions_FunctionId",
                table: "RoleFunctions");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "RoleFunctions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFrameworkApi.Module.Migrations
{
    /// <inheritdoc />
    public partial class alterSystemConfigurationAddDefaultRoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppBaseUrl",
                table: "SystemConfigurations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultRoleId",
                table: "SystemConfigurations",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigurations_DefaultRoleId",
                table: "SystemConfigurations",
                column: "DefaultRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemConfigurations_AspNetRoles_DefaultRoleId",
                table: "SystemConfigurations",
                column: "DefaultRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemConfigurations_AspNetRoles_DefaultRoleId",
                table: "SystemConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_SystemConfigurations_DefaultRoleId",
                table: "SystemConfigurations");

            migrationBuilder.DropColumn(
                name: "DefaultRoleId",
                table: "SystemConfigurations");

            migrationBuilder.AlterColumn<string>(
                name: "AppBaseUrl",
                table: "SystemConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}

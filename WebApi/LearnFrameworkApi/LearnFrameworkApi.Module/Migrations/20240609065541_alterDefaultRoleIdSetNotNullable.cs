using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFrameworkApi.Module.Migrations
{
    /// <inheritdoc />
    public partial class alterDefaultRoleIdSetNotNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemConfigurations_AspNetRoles_DefaultRoleId",
                table: "SystemConfigurations");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultRoleId",
                table: "SystemConfigurations",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemConfigurations_AspNetRoles_DefaultRoleId",
                table: "SystemConfigurations",
                column: "DefaultRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemConfigurations_AspNetRoles_DefaultRoleId",
                table: "SystemConfigurations");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultRoleId",
                table: "SystemConfigurations",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemConfigurations_AspNetRoles_DefaultRoleId",
                table: "SystemConfigurations",
                column: "DefaultRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFrameworkApi.Module.Migrations
{
    /// <inheritdoc />
    public partial class changeIsActiveToActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "IsActive",
            //    table: "AspNetUsers",
            //    newName: "Active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Active",
            //    table: "AspNetUsers",
            //    newName: "IsActive");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EkbCulture.AppHost.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Users",
                newName: "IconUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconUrl",
                table: "Users",
                newName: "Icon");
        }
    }
}

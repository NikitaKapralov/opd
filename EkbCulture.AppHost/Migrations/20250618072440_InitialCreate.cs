using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EkbCulture.AppHost.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThirdImage",
                table: "Locations",
                newName: "ThirdImageUrl");

            migrationBuilder.RenameColumn(
                name: "SecondImage",
                table: "Locations",
                newName: "SecondImageUrl");

            migrationBuilder.RenameColumn(
                name: "FirstImage",
                table: "Locations",
                newName: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "FirstImageUrl",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstImageUrl",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "ThirdImageUrl",
                table: "Locations",
                newName: "ThirdImage");

            migrationBuilder.RenameColumn(
                name: "SecondImageUrl",
                table: "Locations",
                newName: "SecondImage");

            migrationBuilder.RenameColumn(
                name: "Ratings",
                table: "Locations",
                newName: "FirstImage");
        }
    }
}

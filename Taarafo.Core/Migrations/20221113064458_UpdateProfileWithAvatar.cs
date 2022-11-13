using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taarafo.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfileWithAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Profiles");
        }
    }
}

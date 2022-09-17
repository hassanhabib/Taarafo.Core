using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taarafo.Core.Migrations
{
    public partial class UpdateEventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Events",
                newName: "UpdatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Events",
                newName: "MyProperty");
        }
    }
}

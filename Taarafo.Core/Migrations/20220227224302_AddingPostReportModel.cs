// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taarafo.Core.Migrations
{
    public partial class AddingPostReportModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostsReported",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsReported", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostsReported_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsReported_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostsReported_PostId",
                table: "PostsReported",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostsReported_ProfileId",
                table: "PostsReported",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsReported");
        }
    }
}

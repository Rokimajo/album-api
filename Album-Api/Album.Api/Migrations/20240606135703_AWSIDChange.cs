using Microsoft.EntityFrameworkCore.Migrations;

namespace Album.Api.Migrations
{
    public partial class AWSIDChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Albums",
                newName: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Albums",
                newName: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBlogg.Migrations
{
    public partial class addColumnToCommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplained",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplained",
                table: "Comments");
        }
    }
}

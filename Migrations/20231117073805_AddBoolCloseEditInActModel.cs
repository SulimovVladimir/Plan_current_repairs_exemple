using Microsoft.EntityFrameworkCore.Migrations;

namespace Plan_current_repairs.Migrations
{
    public partial class AddBoolCloseEditInActModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCloseEditAndRemove",
                table: "ActsOfWorks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCloseEditAndRemove",
                table: "ActsOfWorks");
        }
    }
}

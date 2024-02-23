using Microsoft.EntityFrameworkCore.Migrations;

namespace Plan_current_repairs.Migrations
{
    public partial class AddLinkActToOtherWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActOfWorkOtherWork",
                columns: table => new
                {
                    ActOfWorksIDAct = table.Column<int>(type: "integer", nullable: false),
                    OtherWorksOtherWorkID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActOfWorkOtherWork", x => new { x.ActOfWorksIDAct, x.OtherWorksOtherWorkID });
                    table.ForeignKey(
                        name: "FK_ActOfWorkOtherWork_ActsOfWorks_ActOfWorksIDAct",
                        column: x => x.ActOfWorksIDAct,
                        principalTable: "ActsOfWorks",
                        principalColumn: "IDAct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActOfWorkOtherWork_OtherWorks_OtherWorksOtherWorkID",
                        column: x => x.OtherWorksOtherWorkID,
                        principalTable: "OtherWorks",
                        principalColumn: "OtherWorkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActOfWorkOtherWork_OtherWorksOtherWorkID",
                table: "ActOfWorkOtherWork",
                column: "OtherWorksOtherWorkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActOfWorkOtherWork");
        }
    }
}

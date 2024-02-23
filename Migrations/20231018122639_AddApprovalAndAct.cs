using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Plan_current_repairs.Migrations
{
    public partial class AddApprovalAndAct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administrator",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "Employees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Post",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StandartNumberAct",
                table: "Departments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActsOfWorks",
                columns: table => new
                {
                    IDAct = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberAct = table.Column<string>(type: "text", nullable: true),
                    DateAct = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EmployeeList = table.Column<string>(type: "text", nullable: true),
                    TitleWork = table.Column<string>(type: "text", nullable: true),
                    ResponsibleForWork_Name = table.Column<string>(type: "text", nullable: true),
                    ResponsibleForWork_Post = table.Column<string>(type: "text", nullable: true),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false),
                    YearID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActsOfWorks", x => x.IDAct);
                    table.ForeignKey(
                        name: "FK_ActsOfWorks_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActsOfWorks_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "YearID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    IDStatus = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleStatus = table.Column<string>(type: "text", nullable: true),
                    AssertionPlan_Array = table.Column<string>(type: "text", nullable: true),
                    Assertion_1_CalendarQuarter_Array = table.Column<string>(type: "text", nullable: true),
                    Assertion_2_CalendarQuarter_Array = table.Column<string>(type: "text", nullable: true),
                    Assertion_3_CalendarQuarter_Array = table.Column<string>(type: "text", nullable: true),
                    Assertion_4_CalendarQuarter_Array = table.Column<string>(type: "text", nullable: true),
                    Blocking_Array = table.Column<string>(type: "text", nullable: true),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false),
                    YearID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.IDStatus);
                    table.ForeignKey(
                        name: "FK_Status_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Status_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "YearID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActOfWorkJornal",
                columns: table => new
                {
                    ActOfWorksIDAct = table.Column<int>(type: "integer", nullable: false),
                    JornalsNumberRecordingID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActOfWorkJornal", x => new { x.ActOfWorksIDAct, x.JornalsNumberRecordingID });
                    table.ForeignKey(
                        name: "FK_ActOfWorkJornal_ActsOfWorks_ActOfWorksIDAct",
                        column: x => x.ActOfWorksIDAct,
                        principalTable: "ActsOfWorks",
                        principalColumn: "IDAct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActOfWorkJornal_Jornals_JornalsNumberRecordingID",
                        column: x => x.JornalsNumberRecordingID,
                        principalTable: "Jornals",
                        principalColumn: "NumberRecordingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActOfWorkJornal_JornalsNumberRecordingID",
                table: "ActOfWorkJornal",
                column: "JornalsNumberRecordingID");

            migrationBuilder.CreateIndex(
                name: "IX_ActsOfWorks_DepartmentID",
                table: "ActsOfWorks",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ActsOfWorks_YearID",
                table: "ActsOfWorks",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Status_DepartmentID",
                table: "Status",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Status_YearID",
                table: "Status",
                column: "YearID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActOfWorkJornal");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "ActsOfWorks");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Post",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "StandartNumberAct",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Administrator",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

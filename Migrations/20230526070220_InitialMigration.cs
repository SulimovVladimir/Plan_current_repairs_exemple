using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Plan_current_repairs.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameDepartment = table.Column<string>(type: "text", nullable: false),
                    DirectorDepartment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameGroup = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupID);
                });

            migrationBuilder.CreateTable(
                name: "Years",
                columns: table => new
                {
                    YearID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Years = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Years", x => x.YearID);
                });

            migrationBuilder.CreateTable(
                name: "DictionarySector",
                columns: table => new
                {
                    DictionarySectorID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionarySector", x => x.DictionarySectorID);
                    table.ForeignKey(
                        name: "FK_DictionarySector_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Account = table.Column<string>(type: "text", nullable: false),
                    Administrator = table.Column<bool>(type: "boolean", nullable: false),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    WorkID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameOfWork = table.Column<string>(type: "text", nullable: false),
                    Discription = table.Column<string>(type: "text", nullable: false),
                    Periodicity = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    IntegerValue = table.Column<bool>(type: "boolean", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    TypeRecords = table.Column<string>(type: "text", nullable: true),
                    Parameter_1 = table.Column<string>(type: "text", nullable: true),
                    Parameter_2 = table.Column<string>(type: "text", nullable: true),
                    Parameter_3 = table.Column<string>(type: "text", nullable: true),
                    GroupNameOfWorksID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.WorkID);
                    table.ForeignKey(
                        name: "FK_Works_Groups_GroupNameOfWorksID",
                        column: x => x.GroupNameOfWorksID,
                        principalTable: "Groups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherWorks",
                columns: table => new
                {
                    OtherWorkID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameOtherWork = table.Column<string>(type: "text", nullable: true),
                    DiscriptionOtherWork = table.Column<string>(type: "text", nullable: true),
                    PeriodicityOtherWork = table.Column<string>(type: "text", nullable: true),
                    UnitOtherWork = table.Column<string>(type: "text", nullable: true),
                    NoteOtherWork = table.Column<string>(type: "text", nullable: true),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false),
                    YearID = table.Column<int>(type: "integer", nullable: false),
                    OtherFactValue = table.Column<string>(type: "text", nullable: true),
                    OtherPlanValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherWorks", x => x.OtherWorkID);
                    table.ForeignKey(
                        name: "FK_OtherWorks_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OtherWorks_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "YearID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jornals",
                columns: table => new
                {
                    NumberRecordingID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sector = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    NameOfWorksID = table.Column<int>(type: "integer", nullable: false),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false),
                    YearID = table.Column<int>(type: "integer", nullable: false),
                    CreatedInPlan = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornals", x => x.NumberRecordingID);
                    table.ForeignKey(
                        name: "FK_Jornals_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jornals_Works_NameOfWorksID",
                        column: x => x.NameOfWorksID,
                        principalTable: "Works",
                        principalColumn: "WorkID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jornals_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "YearID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactValue",
                columns: table => new
                {
                    FactID = table.Column<int>(type: "integer", nullable: false),
                    FactForJanuary = table.Column<float>(type: "real", nullable: false),
                    FactForFebruary = table.Column<float>(type: "real", nullable: false),
                    FactForMarch = table.Column<float>(type: "real", nullable: false),
                    FactForApril = table.Column<float>(type: "real", nullable: false),
                    FactForMay = table.Column<float>(type: "real", nullable: false),
                    FactForJune = table.Column<float>(type: "real", nullable: false),
                    FactForJuly = table.Column<float>(type: "real", nullable: false),
                    FactForAugust = table.Column<float>(type: "real", nullable: false),
                    FactForSeptember = table.Column<float>(type: "real", nullable: false),
                    FactForOctober = table.Column<float>(type: "real", nullable: false),
                    FactForNovember = table.Column<float>(type: "real", nullable: false),
                    FactForDecember = table.Column<float>(type: "real", nullable: false),
                    JornalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactValue", x => x.FactID);
                    table.ForeignKey(
                        name: "FK_FactValue_Jornals_JornalID",
                        column: x => x.JornalID,
                        principalTable: "Jornals",
                        principalColumn: "NumberRecordingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters_1",
                columns: table => new
                {
                    Parameter_1ID = table.Column<int>(type: "integer", nullable: false),
                    PlanParameter_1ForJanuary = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForFebruary = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForMarch = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForApril = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForMay = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForJune = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForJuly = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForAugust = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForSeptember = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForOctober = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForNovember = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_1ForDecember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForJanuary = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForFebruary = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForMarch = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForApril = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForMay = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForJune = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForJuly = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForAugust = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForSeptember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForOctober = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForNovember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_1ForDecember = table.Column<float>(type: "real", nullable: false),
                    JornalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters_1", x => x.Parameter_1ID);
                    table.ForeignKey(
                        name: "FK_Parameters_1_Jornals_JornalID",
                        column: x => x.JornalID,
                        principalTable: "Jornals",
                        principalColumn: "NumberRecordingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters_2",
                columns: table => new
                {
                    Parameter_2ID = table.Column<int>(type: "integer", nullable: false),
                    PlanParameter_2ForJanuary = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForFebruary = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForMarch = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForApril = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForMay = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForJune = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForJuly = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForAugust = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForSeptember = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForOctober = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForNovember = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_2ForDecember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForJanuary = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForFebruary = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForMarch = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForApril = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForMay = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForJune = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForJuly = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForAugust = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForSeptember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForOctober = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForNovember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_2ForDecember = table.Column<float>(type: "real", nullable: false),
                    JornalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters_2", x => x.Parameter_2ID);
                    table.ForeignKey(
                        name: "FK_Parameters_2_Jornals_JornalID",
                        column: x => x.JornalID,
                        principalTable: "Jornals",
                        principalColumn: "NumberRecordingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters_3",
                columns: table => new
                {
                    Parameter_3ID = table.Column<int>(type: "integer", nullable: false),
                    PlanParameter_3ForJanuary = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForFebruary = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForMarch = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForApril = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForMay = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForJune = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForJuly = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForAugust = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForSeptember = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForOctober = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForNovember = table.Column<float>(type: "real", nullable: false),
                    PlanParameter_3ForDecember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForJanuary = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForFebruary = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForMarch = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForApril = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForMay = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForJune = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForJuly = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForAugust = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForSeptember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForOctober = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForNovember = table.Column<float>(type: "real", nullable: false),
                    FactParameter_3ForDecember = table.Column<float>(type: "real", nullable: false),
                    JornalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters_3", x => x.Parameter_3ID);
                    table.ForeignKey(
                        name: "FK_Parameters_3_Jornals_JornalID",
                        column: x => x.JornalID,
                        principalTable: "Jornals",
                        principalColumn: "NumberRecordingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanValue",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "integer", nullable: false),
                    PlanForJanuary = table.Column<float>(type: "real", nullable: false),
                    PlanForFebruary = table.Column<float>(type: "real", nullable: false),
                    PlanForMarch = table.Column<float>(type: "real", nullable: false),
                    PlanForApril = table.Column<float>(type: "real", nullable: false),
                    PlanForMay = table.Column<float>(type: "real", nullable: false),
                    PlanForJune = table.Column<float>(type: "real", nullable: false),
                    PlanForJuly = table.Column<float>(type: "real", nullable: false),
                    PlanForAugust = table.Column<float>(type: "real", nullable: false),
                    PlanForSeptember = table.Column<float>(type: "real", nullable: false),
                    PlanForOctober = table.Column<float>(type: "real", nullable: false),
                    PlanForNovember = table.Column<float>(type: "real", nullable: false),
                    PlanForDecember = table.Column<float>(type: "real", nullable: false),
                    JornalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanValue", x => x.PlanID);
                    table.ForeignKey(
                        name: "FK_PlanValue_Jornals_JornalID",
                        column: x => x.JornalID,
                        principalTable: "Jornals",
                        principalColumn: "NumberRecordingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "ID", "DirectorDepartment", "NameDepartment" },
                values: new object[] { 1, "Есипов Дмитрий Владимирович", "Администрация" });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "GroupID", "NameGroup" },
                values: new object[] { 1, "Дополнительные работы, не предусмотренные планом" });

            migrationBuilder.CreateIndex(
                name: "IX_DictionarySector_DepartmentID",
                table: "DictionarySector",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_FactValue_JornalID",
                table: "FactValue",
                column: "JornalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jornals_DepartmentID",
                table: "Jornals",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Jornals_NameOfWorksID",
                table: "Jornals",
                column: "NameOfWorksID");

            migrationBuilder.CreateIndex(
                name: "IX_Jornals_YearID",
                table: "Jornals",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_OtherWorks_DepartmentID",
                table: "OtherWorks",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_OtherWorks_YearID",
                table: "OtherWorks",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_1_JornalID",
                table: "Parameters_1",
                column: "JornalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_2_JornalID",
                table: "Parameters_2",
                column: "JornalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_3_JornalID",
                table: "Parameters_3",
                column: "JornalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanValue_JornalID",
                table: "PlanValue",
                column: "JornalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Works_GroupNameOfWorksID",
                table: "Works",
                column: "GroupNameOfWorksID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionarySector");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "FactValue");

            migrationBuilder.DropTable(
                name: "OtherWorks");

            migrationBuilder.DropTable(
                name: "Parameters_1");

            migrationBuilder.DropTable(
                name: "Parameters_2");

            migrationBuilder.DropTable(
                name: "Parameters_3");

            migrationBuilder.DropTable(
                name: "PlanValue");

            migrationBuilder.DropTable(
                name: "Jornals");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Years");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class Currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cur_ID = table.Column<int>(type: "int", nullable: false),
                    Cur_ParentID = table.Column<int>(type: "int", nullable: true),
                    Cur_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Name_Bel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Name_Eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_QuotName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_QuotName_Bel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_QuotName_Eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_NameMulti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Name_BelMulti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Name_EngMulti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cur_Scale = table.Column<int>(type: "int", nullable: false),
                    Cur_Periodicity = table.Column<int>(type: "int", nullable: false),
                    Cur_DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cur_DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cur_Scale = table.Column<int>(type: "int", nullable: false),
                    Cur_OfficialRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyId",
                table: "CurrencyRates",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}

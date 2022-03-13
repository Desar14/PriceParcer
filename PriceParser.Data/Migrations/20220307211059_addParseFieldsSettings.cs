using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class addParseFieldsSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParseCurrencyAttributeName",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParsePriceAttributeName",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParseCurrencyAttributeName",
                table: "MarketSites");

            migrationBuilder.DropColumn(
                name: "ParsePriceAttributeName",
                table: "MarketSites");
        }
    }
}

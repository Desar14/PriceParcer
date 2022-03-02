using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParcer.Data.Migrations
{
    public partial class ParseParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParseType",
                table: "MarketSites",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParseCurrencyPath",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParsePricePath",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParseCurrencyPath",
                table: "MarketSites");

            migrationBuilder.DropColumn(
                name: "ParsePricePath",
                table: "MarketSites");

            migrationBuilder.AlterColumn<string>(
                name: "ParseType",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

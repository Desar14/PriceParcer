using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class PaseFailFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ParseError",
                table: "ProductPricesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParseError",
                table: "ProductPricesHistory");
        }
    }
}

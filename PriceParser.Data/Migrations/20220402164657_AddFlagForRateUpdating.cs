using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class AddFlagForRateUpdating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UpdateRates",
                table: "Currencies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateRates",
                table: "Currencies");
        }
    }
}

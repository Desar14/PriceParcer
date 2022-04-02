using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class CurrencyInPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "ProductPricesHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricesHistory_CurrencyId",
                table: "ProductPricesHistory",
                column: "CurrencyId");

            migrationBuilder.UpdateData(table: "ProductPricesHistory", "CurrencyCode", "BYN", "CurrencyId", "542A6CB5-EDC8-4F73-1097-08DA14B6DEC3");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPricesHistory_Currencies_CurrencyId",
                table: "ProductPricesHistory",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPricesHistory_Currencies_CurrencyId",
                table: "ProductPricesHistory");

            migrationBuilder.DropIndex(
                name: "IX_ProductPricesHistory_CurrencyId",
                table: "ProductPricesHistory");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ProductPricesHistory");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class RenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_ProductsFromSites_ProductId",
                table: "ProductPrices");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductPrices",
                newName: "ProductFromSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_ProductFromSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_ProductsFromSites_ProductFromSiteId",
                table: "ProductPrices",
                column: "ProductFromSiteId",
                principalTable: "ProductsFromSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_ProductsFromSites_ProductFromSiteId",
                table: "ProductPrices");

            migrationBuilder.RenameColumn(
                name: "ProductFromSiteId",
                table: "ProductPrices",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_ProductFromSiteId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_ProductsFromSites_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                principalTable: "ProductsFromSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

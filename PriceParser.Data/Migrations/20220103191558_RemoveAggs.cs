using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class RemoveAggs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_ProductsFromSites_ProductFromSiteId",
                table: "ProductPrices");

            migrationBuilder.DropTable(
                name: "ProductsAggregations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.RenameTable(
                name: "ProductPrices",
                newName: "ProductPricesHistory");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_ProductFromSiteId",
                table: "ProductPricesHistory",
                newName: "IX_ProductPricesHistory_ProductFromSiteId");

            migrationBuilder.AddColumn<double>(
                name: "AveragePriceNow",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AveragePriceOverall",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "AverageScore",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BestPriceNow",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BestPriceOverall",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAggregate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ParseType",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPricesHistory",
                table: "ProductPricesHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPricesHistory_ProductsFromSites_ProductFromSiteId",
                table: "ProductPricesHistory",
                column: "ProductFromSiteId",
                principalTable: "ProductsFromSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPricesHistory_ProductsFromSites_ProductFromSiteId",
                table: "ProductPricesHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPricesHistory",
                table: "ProductPricesHistory");

            migrationBuilder.DropColumn(
                name: "AveragePriceNow",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AveragePriceOverall",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AverageScore",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BestPriceNow",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BestPriceOverall",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastAggregate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ParseType",
                table: "MarketSites");

            migrationBuilder.RenameTable(
                name: "ProductPricesHistory",
                newName: "ProductPrices");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPricesHistory_ProductFromSiteId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_ProductFromSiteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductsAggregations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AveragePriceNow = table.Column<double>(type: "float", nullable: false),
                    AveragePriceOverall = table.Column<double>(type: "float", nullable: false),
                    AverageScore = table.Column<float>(type: "real", nullable: false),
                    BestPriceNow = table.Column<double>(type: "float", nullable: false),
                    BestPriceOverall = table.Column<double>(type: "float", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastAggregate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsAggregations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsAggregations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsAggregations_ProductId",
                table: "ProductsAggregations",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_ProductsFromSites_ProductFromSiteId",
                table: "ProductPrices",
                column: "ProductFromSiteId",
                principalTable: "ProductsFromSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

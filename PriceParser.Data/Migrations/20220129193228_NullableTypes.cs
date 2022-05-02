using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParser.Data.Migrations
{
    public partial class NullableTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsFromSites_AspNetUsers_CreatedByUserId",
                table: "ProductsFromSites");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "ProductsFromSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ParseSchedule",
                table: "ProductsFromSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "ProductsFromSites",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageData",
                table: "Products",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsFromSites_AspNetUsers_CreatedByUserId",
                table: "ProductsFromSites",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            SeedDatabase(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsFromSites_AspNetUsers_CreatedByUserId",
                table: "ProductsFromSites");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "ProductsFromSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParseSchedule",
                table: "ProductsFromSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "ProductsFromSites",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageData",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsFromSites_AspNetUsers_CreatedByUserId",
                table: "ProductsFromSites",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        private static void SeedDatabase(MigrationBuilder migrationBuilder)
        {
            /*Users*/
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] { "0232FDBA-3C69-4250-AEA5-8CC69C3F87BD", "internalUser", false, false, false, false, 0 });

            /*sites*/
            migrationBuilder.InsertData(
                table: "MarketSites",
                columns: new[] { "Id", "Name", "IsAvailable", "Created" },
                values: new object[] { new Guid("D770D261-962C-4241-9200-6FC6AE7D56C2"), "21vek.by", true, DateTime.Now });

            migrationBuilder.InsertData(
                table: "MarketSites",
                columns: new[] { "Id", "Name", "IsAvailable", "Created" },
                values: new object[] { new Guid("343B4267-ECE3-49DA-BE0D-AAFA23DB1657"), "amd.by", true, DateTime.Now });

            migrationBuilder.InsertData(
                table: "MarketSites",
                columns: new[] { "Id", "Name", "IsAvailable", "Created" },
                values: new object[] { new Guid("714263FE-1E66-48C5-92CA-3B5C8CC26F2F"), "sli.by", true, DateTime.Now });

            /*products*/
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore", "UseExternalImage" },
                values: new object[] { new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), "PALIT GeForce RTX 3070 GamingPro 8GB GDDR6", "GPU", "awesome rtx 3070", false, 3000, 2000, 5, false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore", "UseExternalImage" },
                values: new object[] { new Guid("51B3CD3E-2414-41ED-AA61-D56AED2313BE"), "PALIT GeForce RTX 3070 GameRock 8GB GDDR6", "GPU", "awesome rtx 3070 #2", false, 3010, 2010, 4.5, false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore", "UseExternalImage" },
                values: new object[] { new Guid("EDD78342-5065-4025-906B-807FFB88747D"), "SSD Samsung 870 Evo 500GB MZ-77E500BW", "SSD", "awesome samsung ssd", false, 250, 220, 4.1, false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore", "UseExternalImage" },
                values: new object[] { new Guid("FD607161-5823-417F-9A05-1661B58CD35A"), "be quiet! Dark Power Pro 12 1200W BN311", "Power supply", "awesome PS by be quiet", false, 1300, 1000, 4.8, false });


            /*links*/
            migrationBuilder.InsertData(
                table: "ProductsFromSites",
                columns: new[] { "Id", "ProductId", "SiteId", "Path", "DoNotParse", "ParseSchedule", "Created", "CreatedByUserId" },
                values: new object[] { new Guid("41140870-7145-4F83-BA9B-5F6EAA6E6CEB"), new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), new Guid("D770D261-962C-4241-9200-6FC6AE7D56C2"), "https://www.21vek.by/graphic_cards/geforcertx3070gamingprooc8gbgddr6ne63070s19p21041a_palit.html", false, "", DateTime.Now, "0232FDBA-3C69-4250-AEA5-8CC69C3F87BD" });

            migrationBuilder.InsertData(
                table: "ProductsFromSites",
                columns: new[] { "Id", "ProductId", "SiteId", "Path", "DoNotParse", "ParseSchedule", "Created", "CreatedByUserId" },
                values: new object[] { new Guid("88A86A88-C577-4645-938A-DFCCFF37BE25"), new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), new Guid("714263FE-1E66-48C5-92CA-3B5C8CC26F2F"), "http://sli.by/catalog/zms16-komputernaya-tehnika/zms542-komplektuushie/zms1310-videokarty/i1872603.html", false, "", DateTime.Now, "0232FDBA-3C69-4250-AEA5-8CC69C3F87BD" });

            migrationBuilder.InsertData(
                table: "ProductsFromSites",
                columns: new[] { "Id", "ProductId", "SiteId", "Path", "DoNotParse", "ParseSchedule", "Created", "CreatedByUserId" },
                values: new object[] { new Guid("4E6CD14F-9831-4571-9E77-9AEAACE086D5"), new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), new Guid("343B4267-ECE3-49DA-BE0D-AAFA23DB1657"), "https://www.amd.by/videokarty/palit-geforce-rtx-3070-gamingpro-oc-v1-8gb-gddr6/", false, "", DateTime.Now, "0232FDBA-3C69-4250-AEA5-8CC69C3F87BD" });
        }
    }
}

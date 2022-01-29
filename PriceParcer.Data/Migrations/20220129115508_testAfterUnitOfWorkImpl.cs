using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceParcer.Data.Migrations
{
    public partial class testAfterUnitOfWorkImpl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SitePassword",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SiteLogin",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ParseType",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthType",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
            
            /*sites*/
            migrationBuilder.InsertData(
                table: "MarketSites",
                columns: new[] { "Id", "Name", "IsAvailable", "Created"},
                values: new object[] { new Guid("D770D261-962C-4241-9200-6FC6AE7D56C2"), "21vek.by", true, DateTime.Now});

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
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore"},
                values: new object[] { new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), "PALIT GeForce RTX 3070 GamingPro 8GB GDDR6", "GPU", "awesome rtx 3070", false, 3000, 2000, 5 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore" },
                values: new object[] { new Guid("51B3CD3E-2414-41ED-AA61-D56AED2313BE"), "PALIT GeForce RTX 3070 GameRock 8GB GDDR6", "GPU", "awesome rtx 3070 #2", false, 3010, 2010, 4.5 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore" },
                values: new object[] { new Guid("EDD78342-5065-4025-906B-807FFB88747D"), "SSD Samsung 870 Evo 500GB MZ-77E500BW", "SSD", "awesome samsung ssd", false, 250, 220, 4.1 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Category", "Description", "Hidden", "BestPriceNow", "BestPriceOverall", "AverageScore" },
                values: new object[] { new Guid("FD607161-5823-417F-9A05-1661B58CD35A"), "be quiet! Dark Power Pro 12 1200W BN311", "Power supply", "awesome PS by be quiet", false, 1300, 1000, 4.8 });
            

            /*links*/
            migrationBuilder.InsertData(
                table: "ProductsFromSites",
                columns: new[] { "Id", "ProductId", "SiteId", "Path", "DoNotParse", "ParseSchedule", "Created", "CreatedByUserId" },
                values: new object[] { new Guid("41140870-7145-4F83-BA9B-5F6EAA6E6CEB"), new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), new Guid("D770D261-962C-4241-9200-6FC6AE7D56C2"), "https://www.21vek.by/graphic_cards/geforcertx3070gamingprooc8gbgddr6ne63070s19p21041a_palit.html", false, "", DateTime.Now, Guid.Empty });

            migrationBuilder.InsertData(
                table: "ProductsFromSites",
                columns: new[] { "Id", "ProductId", "SiteId", "Path", "DoNotParse", "ParseSchedule", "Created", "CreatedByUserId" },
                values: new object[] { new Guid("88A86A88-C577-4645-938A-DFCCFF37BE25"), new Guid("C6B597B0-FABA-4472-8AB5-2E685705A55E"), new Guid("714263FE-1E66-48C5-92CA-3B5C8CC26F2F"), "http://sli.by/catalog/zms16-komputernaya-tehnika/zms542-komplektuushie/zms1310-videokarty/i1872603.html", false, "", DateTime.Now, Guid.Empty });

            migrationBuilder.InsertData(
                table: "ProductsFromSites",
                columns: new[] { "Id", "ProductId", "SiteId", "Path", "DoNotParse", "ParseSchedule", "Created", "CreatedByUserId" },
                values: new object[] { new Guid("4E6CD14F-9831-4571-9E77-9AEAACE086D5"), new Guid("343B4267-ECE3-49DA-BE0D-AAFA23DB1657"), new Guid("714263FE-1E66-48C5-92CA-3B5C8CC26F2F"), "https://www.amd.by/videokarty/palit-geforce-rtx-3070-gamingpro-oc-v1-8gb-gddr6/", false, "", DateTime.Now, Guid.Empty });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SitePassword",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SiteLogin",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParseType",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthType",
                table: "MarketSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

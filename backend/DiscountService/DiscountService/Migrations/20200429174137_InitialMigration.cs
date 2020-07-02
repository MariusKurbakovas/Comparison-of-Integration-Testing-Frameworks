using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountService.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscountCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalPrice = table.Column<decimal>(nullable: false),
                    LoyaltyType = table.Column<string>(nullable: false),
                    CustomerType = table.Column<string>(nullable: false),
                    DiscountOnProduct = table.Column<decimal>(nullable: true),
                    DiscountedPrice = table.Column<decimal>(nullable: false),
                    MoneySavedInCard = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCalculations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountCalculations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountService.Migrations
{
    public partial class AddDiscountPercent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "DiscountCalculations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "DiscountCalculations");
        }
    }
}

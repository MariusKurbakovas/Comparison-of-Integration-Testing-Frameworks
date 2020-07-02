using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountService.Migrations
{
    public partial class addCreatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatonOn",
                table: "DiscountCalculations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatonOn",
                table: "DiscountCalculations");
        }
    }
}

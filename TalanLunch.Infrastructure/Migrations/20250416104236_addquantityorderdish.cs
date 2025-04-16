using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalanLunch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addquantityorderdish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderDishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

           

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderDishes");

            migrationBuilder.DropColumn(
                name: "IsMenuOfTheDay",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "DishQuantity",
                table: "MenuDishes");

        }
    }
}

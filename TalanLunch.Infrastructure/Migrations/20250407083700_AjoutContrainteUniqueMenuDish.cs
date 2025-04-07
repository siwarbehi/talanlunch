using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalanLunch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjoutContrainteUniqueMenuDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuDishes_Dishes_DishId",
                table: "MenuDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuDishes_Menus_MenuId",
                table: "MenuDishes");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_MenuId_DishId",
                table: "MenuDishes",
                columns: new[] { "MenuId", "DishId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDishes_Dishes_DishId",
                table: "MenuDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDishes_Menus_MenuId",
                table: "MenuDishes",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuDishes_Dishes_DishId",
                table: "MenuDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuDishes_Menus_MenuId",
                table: "MenuDishes");

            migrationBuilder.DropIndex(
                name: "IX_MenuDishes_MenuId_DishId",
                table: "MenuDishes");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDishes_Dishes_DishId",
                table: "MenuDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDishes_Menus_MenuId",
                table: "MenuDishes",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

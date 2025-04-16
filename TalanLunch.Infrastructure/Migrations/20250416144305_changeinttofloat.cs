using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalanLunch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeinttofloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "CurrentRating",
                table: "Dishes",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CurrentRating",
                table: "Dishes",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}

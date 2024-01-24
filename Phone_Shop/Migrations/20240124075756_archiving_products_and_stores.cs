using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Migrations
{
    /// <inheritdoc />
    public partial class archiving_products_and_stores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Store",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Product");
        }
    }
}

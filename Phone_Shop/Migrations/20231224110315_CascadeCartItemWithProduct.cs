using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Migrations
{
    /// <inheritdoc />
    public partial class CascadeCartItemWithProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Product_ProductId",
                table: "ShoppingCartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Product_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Product_ProductId",
                table: "ShoppingCartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Product_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}

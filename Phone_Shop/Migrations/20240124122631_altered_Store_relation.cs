using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Migrations
{
    /// <inheritdoc />
    public partial class altered_Store_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_AspNetUsers_SellerId",
                table: "Store");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_AspNetUsers_SellerId",
                table: "Store",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_AspNetUsers_SellerId",
                table: "Store");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_AspNetUsers_SellerId",
                table: "Store",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

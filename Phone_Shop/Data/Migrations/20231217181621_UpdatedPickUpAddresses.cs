using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPickUpAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "PickupAddress");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "PickupAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

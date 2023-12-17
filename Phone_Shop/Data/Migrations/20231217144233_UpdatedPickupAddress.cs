using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPickupAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PickupAddress");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "PickupAddress");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "PickupAddress");
        }
    }
}

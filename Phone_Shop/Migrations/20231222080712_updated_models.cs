using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Migrations
{
    /// <inheritdoc />
    public partial class updated_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "PickupAddress",
                newName: "AdditionalInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalInfo",
                table: "PickupAddress",
                newName: "Description");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCityAndGovernateInAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "PickupAddress");

            migrationBuilder.DropColumn(
                name: "Governace",
                table: "PickupAddress");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "PickupAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "PickupAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PickupAddress_CityId",
                table: "PickupAddress",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupAddress_GovernorateId",
                table: "PickupAddress",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PickupAddress_Cities_CityId",
                table: "PickupAddress",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PickupAddress_Governorates_GovernorateId",
                table: "PickupAddress",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickupAddress_Cities_CityId",
                table: "PickupAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_PickupAddress_Governorates_GovernorateId",
                table: "PickupAddress");

            migrationBuilder.DropIndex(
                name: "IX_PickupAddress_CityId",
                table: "PickupAddress");

            migrationBuilder.DropIndex(
                name: "IX_PickupAddress_GovernorateId",
                table: "PickupAddress");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "PickupAddress");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "PickupAddress");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governace",
                table: "PickupAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

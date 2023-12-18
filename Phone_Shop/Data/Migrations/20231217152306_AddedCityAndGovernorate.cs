using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCityAndGovernorate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    governorate_name_ar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    governorate_name_en = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    governorate_id = table.Column<int>(type: "int", nullable: false),
                    city_name_ar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city_name_en = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cities_Governorates_governorate_id",
                        column: x => x.governorate_id,
                        principalTable: "Governorates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_governorate_id",
                table: "Cities",
                column: "governorate_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Governorates");
        }
    }
}

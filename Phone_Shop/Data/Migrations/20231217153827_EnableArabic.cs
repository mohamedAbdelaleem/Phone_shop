using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phone_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnableArabic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase(
                collation: "Arabic_CI_AS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase(
                oldCollation: "Arabic_CI_AS");
        }
    }
}

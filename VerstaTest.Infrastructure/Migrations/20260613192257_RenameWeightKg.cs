using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VerstaTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameWeightKg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Orders",
                newName: "WeightKg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "Orders",
                newName: "Weight");
        }
    }
}

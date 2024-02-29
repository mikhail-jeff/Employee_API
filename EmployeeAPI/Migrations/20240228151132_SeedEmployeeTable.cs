using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Country", "CreatedDate", "Job", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "China", null, "Tiktokerist", "Zhiji Wang", null },
                    { 2, "South Korea", null, "Dancerist", "Sandara Park", null },
                    { 3, "Vietnam", null, "Dancerist", "Bert Nguyen", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

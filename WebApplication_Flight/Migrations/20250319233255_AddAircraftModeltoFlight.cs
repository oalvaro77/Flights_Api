using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication_Flight.Migrations
{
    /// <inheritdoc />
    public partial class AddAircraftModeltoFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AircraftModel",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AircraftModel",
                table: "Flights");
        }
    }
}

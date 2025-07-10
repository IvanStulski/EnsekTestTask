using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnsekTestTask.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixMeterReadValueTypeToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MeterReadValue",
                table: "Meters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MeterReadValue",
                table: "Meters",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}

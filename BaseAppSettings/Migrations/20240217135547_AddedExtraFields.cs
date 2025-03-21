using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseAppSettings.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestHeaders",
                table: "ApiLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponseHeaders",
                table: "ApiLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResponseStatusCode",
                table: "ApiLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestHeaders",
                table: "ApiLogs");

            migrationBuilder.DropColumn(
                name: "ResponseHeaders",
                table: "ApiLogs");

            migrationBuilder.DropColumn(
                name: "ResponseStatusCode",
                table: "ApiLogs");
        }
    }
}

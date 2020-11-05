using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDueDateTracker.Migrations
{
    public partial class ExtensionCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExtensionCount",
                table: "borrow",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -4,
                column: "ExtensionCount",
                value: 1);

            migrationBuilder.UpdateData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -2,
                column: "ExtensionCount",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtensionCount",
                table: "borrow");
        }
    }
}

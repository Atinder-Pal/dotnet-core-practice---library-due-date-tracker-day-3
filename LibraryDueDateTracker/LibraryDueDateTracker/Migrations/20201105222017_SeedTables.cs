using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDueDateTracker.Migrations
{
    public partial class SeedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "author",
                columns: new[] { "ID", "BirthDate", "DeathDate", "Name" },
                values: new object[,]
                {
                    { -1, new DateTime(1975, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Brandon Sanderson" },
                    { -2, new DateTime(1946, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Philip Pullman" },
                    { -3, new DateTime(1965, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Eoin Colfer" },
                    { -4, new DateTime(1835, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1910, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mark Twain" },
                    { -5, new DateTime(1965, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "J.K. Rowling" }
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "ID", "AuthorID", "PublicationDate", "Title" },
                values: new object[,]
                {
                    { -4, -4, new DateTime(1872, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Roughing It" },
                    { -5, -5, new DateTime(2002, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and the Goblet of Fire" },
                    { -1, -5, new DateTime(2013, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Casual Vacancy" },
                    { -2, -5, new DateTime(2017, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hogwarts Library" },
                    { -3, -5, new DateTime(2020, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Ickabog" }
                });

            migrationBuilder.InsertData(
                table: "borrow",
                columns: new[] { "ID", "BookID", "CheckedOutDate", "DueDate", "ReturnedDate" },
                values: new object[,]
                {
                    { -4, -5, new DateTime(2020, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -3, -1, new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { -2, -2, new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -1, -3, new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "author",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "author",
                keyColumn: "ID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "author",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "book",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "author",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "book",
                keyColumn: "ID",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "book",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "book",
                keyColumn: "ID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "book",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "author",
                keyColumn: "ID",
                keyValue: -5);
        }
    }
}

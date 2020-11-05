using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDueDateTracker.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    DeathDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorID = table.Column<int>(type: "int(10)", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    PublicationDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Book_Author",
                        column: x => x.AuthorID,
                        principalTable: "author",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "borrow",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookID = table.Column<int>(type: "int(10)", nullable: false),
                    CheckedOutDate = table.Column<DateTime>(type: "date", nullable: false),
                    DueDate = table.Column<DateTime>(type: "date", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_borrow", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Borrow_Book",
                        column: x => x.BookID,
                        principalTable: "book",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "FK_Book_Author",
                table: "book",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "FK_Borrow_Book",
                table: "borrow",
                column: "BookID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "borrow");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "author");
        }
    }
}

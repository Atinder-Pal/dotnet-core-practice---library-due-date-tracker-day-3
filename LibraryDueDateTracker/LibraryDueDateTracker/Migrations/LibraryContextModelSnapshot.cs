﻿// <auto-generated />
using System;
using LibraryDueDateTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LibraryDueDateTracker.Migrations
{
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LibraryDueDateTracker.Models.Author", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DeathDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(60)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("ID");

                    b.ToTable("author");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            BirthDate = new DateTime(1975, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Brandon Sanderson"
                        },
                        new
                        {
                            ID = -2,
                            BirthDate = new DateTime(1946, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Philip Pullman"
                        },
                        new
                        {
                            ID = -3,
                            BirthDate = new DateTime(1965, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Eoin Colfer"
                        },
                        new
                        {
                            ID = -4,
                            BirthDate = new DateTime(1835, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DeathDate = new DateTime(1910, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mark Twain"
                        },
                        new
                        {
                            ID = -5,
                            BirthDate = new DateTime(1965, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "J.K. Rowling"
                        });
                });

            modelBuilder.Entity("LibraryDueDateTracker.Models.Book", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean");

                    b.Property<int>("AuthorID")
                        .HasColumnType("int(10)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID")
                        .HasName("FK_Book_Author");

                    b.ToTable("book");

                    b.HasData(
                        new
                        {
                            ID = -5,
                            Archived = false,
                            AuthorID = -5,
                            PublicationDate = new DateTime(2002, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Harry Potter and the Goblet of Fire"
                        },
                        new
                        {
                            ID = -1,
                            Archived = false,
                            AuthorID = -5,
                            PublicationDate = new DateTime(2013, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Casual Vacancy"
                        },
                        new
                        {
                            ID = -2,
                            Archived = false,
                            AuthorID = -5,
                            PublicationDate = new DateTime(2017, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Hogwarts Library"
                        },
                        new
                        {
                            ID = -3,
                            Archived = false,
                            AuthorID = -5,
                            PublicationDate = new DateTime(2020, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Ickabog"
                        },
                        new
                        {
                            ID = -4,
                            Archived = false,
                            AuthorID = -4,
                            PublicationDate = new DateTime(1872, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Roughing It"
                        });
                });

            modelBuilder.Entity("LibraryDueDateTracker.Models.Borrow", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<int>("BookID")
                        .HasColumnType("int(10)");

                    b.Property<DateTime>("CheckedOutDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("date");

                    b.Property<int>("ExtensionCount")
                        .HasColumnType("int(10)");

                    b.Property<DateTime?>("ReturnedDate")
                        .HasColumnType("date");

                    b.HasKey("ID");

                    b.HasIndex("BookID")
                        .HasName("FK_Borrow_Book");

                    b.ToTable("borrow");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            BookID = -3,
                            CheckedOutDate = new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DueDate = new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ExtensionCount = 0,
                            ReturnedDate = new DateTime(2020, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ID = -2,
                            BookID = -2,
                            CheckedOutDate = new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DueDate = new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ExtensionCount = 1,
                            ReturnedDate = new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ID = -3,
                            BookID = -1,
                            CheckedOutDate = new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DueDate = new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ExtensionCount = 0
                        },
                        new
                        {
                            ID = -4,
                            BookID = -5,
                            CheckedOutDate = new DateTime(2020, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DueDate = new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ExtensionCount = 1,
                            ReturnedDate = new DateTime(2020, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("LibraryDueDateTracker.Models.Book", b =>
                {
                    b.HasOne("LibraryDueDateTracker.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorID")
                        .HasConstraintName("FK_Book_Author")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryDueDateTracker.Models.Borrow", b =>
                {
                    b.HasOne("LibraryDueDateTracker.Models.Book", "Book")
                        .WithMany("Borrows")
                        .HasForeignKey("BookID")
                        .HasConstraintName("FK_Borrow_Book")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

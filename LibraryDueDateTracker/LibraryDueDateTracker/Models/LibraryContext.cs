using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDueDateTracker.Models
{
    public class LibraryContext : DbContext
    {
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }       
        public virtual DbSet<Borrow> Borrows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connection =
                    "server=localhost;" +
                    "port = 3306;" +
                    "user = root;" +
                    "database = mvc_library;";

                string version = "10.4.14-MariaDB";

                optionsBuilder.UseMySql(connection, x => x.ServerVersion(version));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasData(
                     new Author()
                     {
                         ID = -1,
                         Name = "Brandon Sanderson",
                         BirthDate = new DateTime(1975, 12, 19)
                     },
                     new Author()
                     {
                         ID = -2,
                         Name = "Philip Pullman",
                         BirthDate = new DateTime(1946, 10, 19)
                     },
                     new Author()
                     {
                         ID = -3,
                         Name = "Eoin Colfer",
                         BirthDate = new DateTime(1965, 05, 14)
                     },
                    new Author()
                    {
                        ID = -4,
                        Name = "Mark Twain",
                        BirthDate = new DateTime(1835, 11, 30),
                        DeathDate = new DateTime(1910, 04, 21)
                    },
                   new Author()
                   {
                       ID = -5,
                       Name = "J.K. Rowling",
                       BirthDate = new DateTime(1965, 05, 31)
                   }
                );

            });

            modelBuilder.Entity<Book>(entity =>
            {
                string keyForBook = "FK_" + nameof(Book) +
                    "_" + nameof(Author);

                entity.Property(e => e.Title)
                 .HasCharSet("utf8mb4")
                 .HasCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.AuthorID)
                    .HasName(keyForBook);

                entity.HasOne(thisEntity => thisEntity.Author)
                    .WithMany(parent => parent.Books)
                    .HasForeignKey(thisEntity => thisEntity.AuthorID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName(keyForBook);

                entity.HasData(
                    new Book()
                    {
                        ID = -5,
                        AuthorID = -5,
                        Title = "Harry Potter and the Goblet of Fire",
                        PublicationDate = new DateTime(2002, 09, 15)
                    },
                    new Book()
                    {
                        ID = -1,
                        AuthorID = -5,
                        Title = "The Casual Vacancy",
                        PublicationDate = new DateTime(2013, 07, 12)
                    },
                    new Book()
                    {
                        ID = -2,
                        AuthorID = -5,
                        Title = "Hogwarts Library",
                        PublicationDate = new DateTime(2017, 03, 15)
                    },
                   new Book()
                   {
                       ID = -3,
                       AuthorID = -5,
                       Title = "The Ickabog",
                       PublicationDate = new DateTime(2020, 11, 12)
                   },
                   new Book()
                   {
                       ID = -4,
                       AuthorID = -4,
                       Title = "Roughing It",
                       PublicationDate = new DateTime(1872, 11, 12)
                   }
                );

            });

            modelBuilder.Entity<Borrow>(entity =>
            {
                string keyForBorrow = "FK_" + nameof(Borrow) +
                    "_" + nameof(Book);                

                entity.HasIndex(e => e.BookID)
                    .HasName(keyForBorrow);

                entity.HasOne(thisEntity => thisEntity.Book)
                    .WithMany(parent => parent.Borrows)
                    .HasForeignKey(thisEntity => thisEntity.BookID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName(keyForBorrow);

                entity.HasData(
                    new Borrow()
                    {
                        ID = -1,
                        BookID = -3,
                        CheckedOutDate = new DateTime(2019, 12, 25),
                        DueDate = new DateTime(2020, 01, 08),
                        ReturnedDate = new DateTime(2020, 01, 07),
                        ExtensionCount = 0
                    },
                    new Borrow()
                    {
                        ID = -2,
                        BookID = -2,
                        CheckedOutDate = new DateTime(2019, 12, 25),
                        DueDate = new DateTime(2020, 01, 15),
                        ReturnedDate = new DateTime(2020, 01, 15),
                        ExtensionCount = 1
                    },
                   new Borrow()
                   {
                       ID = -3,
                       BookID = -1,
                       CheckedOutDate = new DateTime(2019, 12, 25),
                       DueDate = new DateTime(2020, 01, 08),
                       ExtensionCount = 0
                   },
                   new Borrow()
                   {
                       ID = -4,
                       BookID = -5,
                       CheckedOutDate = new DateTime(2020, 10, 02),
                       DueDate = new DateTime(2020, 10, 23),
                       ReturnedDate = new DateTime(2020, 10, 22),
                       ExtensionCount = 1
                   }
                );
            });
        }
    }
}

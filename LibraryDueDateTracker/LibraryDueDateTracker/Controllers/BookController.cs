﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDueDateTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryDueDateTracker.Controllers
{
    public class BookController : Controller
    {
        //public static List<Book> Books = new List<Book>();
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult Create(string title, string authorID, string publicationDate)
        {
            ViewBag.authors = AuthorController.GetAuthors();
            if (Request.Method == "POST")
            {
                try
                {
                    Book createdBook = CreateBook(title, authorID, publicationDate);

                    ViewBag.addMessage = $"You have successfully created {createdBook.Title}.";
                }
                catch (ValidationException e)
                {
                    ViewBag.authorID = authorID;
                    ViewBag.bookTitle = title;
                    ViewBag.addMessage = "There exist problem(s) with your submission, see below.";
                    ViewBag.Exception = e;
                    ViewBag.Error = true;
                }
            }
            return View();
        }

        public IActionResult List(string filter)
        {
            if (filter == "overdue")
            {
                ViewBag.status = filter == "overdue" ? "checked" : "";

                ViewBag.list = GetOverdueBooks();

            }
            else if (filter == "archived")
            {
                ViewBag.list = GetArchivedBooks();
            }
            else if (filter == "inStock")
            {
                ViewBag.list = GetInStockBooks();
            }
            else if (filter == "lent")
            {
                ViewBag.list = GetAllLentBooks();
            }
            else
            {
                ViewBag.list = GetBooks();
            }
            return View();
        }

        public IActionResult Details(string id, string op)
        {
            try
            {
                if (op == "delete")
                {
                    DeleteBookByID(id);
                }
                else if (op == "return")
                {
                    ReturnBookByID(id);
                }
                else if (op == "extend")
                {
                    ExtendDueDateForBookByID(id);
                }
                else if (op == "borrow")
                {
                    CreateBorrow(id);
                }

            }
            catch (ValidationException e)
            {
                ViewBag.addMessage = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }
            finally
            {
                ViewBag.bookDetails = GetBookByID(id);
            }

            return View();
        }
        public IActionResult Report()
        {
            ViewBag.dict = GetSum();
            return View();
        }
        //public IActionResult Extend(string id)
        //{
        //    ExtendDueDateForBookByID(id);
        //    return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        //}

        //public IActionResult Return(string id)
        //{
        //    ReturnBookByID(id);
        //    return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        //}

        //public IActionResult Delete(string id)
        //{
        //    DeleteBookByID(id);
        //    return RedirectToAction("List");
        //}
        //public IActionResult Borrow(string id)
        //{
        //    try
        //    {
        //        CreateBorrow(id);
        //        ViewBag.addMessage = "Borrow successful!";
        //    }
        //    catch (ValidationException e)
        //    {                
        //        ViewBag.addMessage = "Borrow unsuccessful, see below.";
        //        ViewBag.Exception = e;
        //        ViewBag.Error = true;

        //        //TempData["Message"] = ViewBag.addMessage;
        //        //TempData["Exception"] = ViewBag.Exception;
        //        //TempData["Error"] = ViewBag.Error;
        //    }
        //    return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        //}

        public Book CreateBook(string title, string authorID, string publicationDate)
        {
            int parsedAuthorID = 0;
            DateTime parsedPublicationdate;
            ValidationException exception = new ValidationException();

            // Trim the values so we don't need to do it a bunch of times later.
            authorID = !(string.IsNullOrWhiteSpace(authorID) || string.IsNullOrEmpty(authorID)) ? authorID.Trim() : null;
            title = !(string.IsNullOrWhiteSpace(title) || string.IsNullOrEmpty(title)) ? title.Trim() : null;
            publicationDate = !(string.IsNullOrWhiteSpace(publicationDate) || string.IsNullOrEmpty(publicationDate)) ? publicationDate.Trim() : null;

            using LibraryContext context = new LibraryContext();
            // No value for authorID.
            if (string.IsNullOrWhiteSpace(authorID))
            {
                exception.ValidationExceptions.Add(new Exception("AuthorID Not Provided"));
            }
            else
            {
                // Author ID fails parse.
                if (!int.TryParse(authorID, out parsedAuthorID))
                {
                    exception.ValidationExceptions.Add(new Exception("Author ID Not Valid"));
                }
                else
                {
                    if (!context.Authors.Any(x => x.ID == parsedAuthorID))
                    {
                        exception.ValidationExceptions.Add(new Exception("Author Does Not Exist"));
                    }
                }
            }

            // No value for title.
            if (string.IsNullOrWhiteSpace(title))
            {
                exception.ValidationExceptions.Add(new Exception("Title Not Provided"));
            }
            else
            {
                if (title.Length > 100)
                {
                    exception.ValidationExceptions.Add(new Exception("Title length exceeds 100 characters"));
                }
                else
                {
                    List<int> authorList = context.Books.Where(x => x.Title.ToLower() == title.ToLower()).Select(x => x.AuthorID).ToList();
                    if (authorList.Any() && authorList.Contains(parsedAuthorID))
                    {
                        exception.ValidationExceptions.Add(new Exception($"The Title already exists for this Author."));
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(publicationDate))
            {
                exception.ValidationExceptions.Add(new Exception("Publication Date  Not Provided"));
            }
            else
            {
                // publicationDate fails parse.
                if (!DateTime.TryParse(publicationDate, out parsedPublicationdate))
                {
                    exception.ValidationExceptions.Add(new Exception("Publication Date Not Valid"));
                }
                else
                {
                    if (parsedPublicationdate > DateTime.Today)
                    {
                        exception.ValidationExceptions.Add(new Exception("Publication date can not be in future."));
                    }
                }
            }
            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            Book newBook = new Book()
            {
                AuthorID = int.Parse(authorID),
                Title = title.Trim(),
                PublicationDate = DateTime.Parse(publicationDate)
            };

            context.Books.Add(newBook);
            context.SaveChanges();
            return newBook;
        }
        public Book GetBookByID(string id)
        {
            int parsedID = 0;
            ValidationException exception = new ValidationException();

            id = !(string.IsNullOrWhiteSpace(id) || string.IsNullOrEmpty(id)) ? id.Trim() : null;
            using LibraryContext context = new LibraryContext();
            if (string.IsNullOrWhiteSpace(id))
            {
                exception.ValidationExceptions.Add(new Exception("Book ID not found"));
            }
            else
            {
                if (!int.TryParse(id, out parsedID))
                {
                    exception.ValidationExceptions.Add(new Exception("Book ID not valid"));
                }
                else
                {
                    if (!context.Books.Any(book => book.ID == parsedID))
                    {
                        exception.ValidationExceptions.Add(new Exception("Book not found"));
                    }
                }
            }
            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }
            return context.Books.Where(book => book.ID == parsedID)
                .Include(book => book.Borrows).Include(x => x.Author).SingleOrDefault();
        }
        public void ExtendDueDateForBookByID(string id)
        {
            BorrowController.ExtendDueDateForBorrowByID(id);
        }

        public void ReturnBookByID(string id)
        {
            BorrowController.ReturnBorrowByID(id);
        }
        public void CreateBorrow(string id)
        {
            BorrowController.CreateBorrow(id);
        }

        public void DeleteBookByID(string id)
        {
            int parsedID = 0;
            ValidationException exception = new ValidationException();
            using LibraryContext context = new LibraryContext();
            // Trim the values so we don't need to do it a bunch of times later.
            id = !(string.IsNullOrWhiteSpace(id) || string.IsNullOrEmpty(id)) ? id.Trim() : null;

            if (string.IsNullOrWhiteSpace(id))
            {
                exception.ValidationExceptions.Add(new Exception("Can't find Book ID"));
            }
            else
            {
                // Book ID fails parse.
                if (!int.TryParse(id, out parsedID))
                {
                    exception.ValidationExceptions.Add(new Exception("Book ID Not Valid"));
                }
                else
                {
                    Book book = context.Books.Where(x => x.ID == parsedID).Include(x => x.Borrows).SingleOrDefault();
                    if (book == null)
                    {
                        exception.ValidationExceptions.Add(new Exception("Book Does Not Exist"));
                    }
                    else
                    {
                        if (book.Archived == true)
                        {
                            exception.ValidationExceptions.Add(new Exception("Book has already been archived/deleted"));
                        }
                        //if book is not returned, it can't be archived
                        else if (book.Borrows.Any())
                        {
                            if (book.Borrows.Any(x => x.ReturnedDate == null))
                            {
                                exception.ValidationExceptions.Add(new Exception("Book has not been returned. Please return it before you delete it."));
                            }
                        }
                    }
                }
            }
            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            context.Books.Where(book => book.ID == parsedID).SingleOrDefault().Archived = true;
            context.SaveChanges();
        }

        public List<Book> GetBooks()
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Where(book => book.Archived == false).Include(book => book.Borrows).Include(x => x.Author).ToList();
        }

        public List<Book> GetOverdueBooks()
        {
            using LibraryContext context = new LibraryContext();
            /*
            List<int> overdueBookIDs = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).Select(x => x.BookID).ToList();
            //Citation
            //https://stackoverflow.com/questions/14257360/linq-select-objects-in-list-where-exists-in-a-b-c/14257379
            //Referenced code from above source to check if element in the list
            List<Book> overdueBooks = context.Books.Where(book => overdueBookIDs.Contains(book.ID)).ToList();
            //End Citation
            */
            List<Book> overdueBooks = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).Select(borrow => borrow.Book).ToList();
            //List<Borrow> overdueBorrow = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).ToList();

            List<Book> overdueBooks1 = context.Books.Include(book => book.Borrows).Include(x => x.Author).Where(book => overdueBooks.Contains(book) && book.Archived == false).ToList();
            return overdueBooks1;
        }
        public List<Book> GetArchivedBooks()
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Where(book => book.Archived == true).Include(book => book.Borrows).Include(x => x.Author).ToList();
        }
        public List<Book> GetInStockBooks()
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Include(book => book.Borrows).Where(b => b.Archived == false && !b.Borrows.Any(b => b.ReturnedDate == null)).Include(x => x.Author).ToList();
        }
        public List<Book> GetAllLentBooks()
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Include(book => book.Borrows).Where(b => b.Archived == false && b.Borrows.Any(b => b.ReturnedDate == null)).Include(x => x.Author).ToList();
        }
        public List<MyType> GetSum()
        {
            
            using LibraryContext context = new LibraryContext();
            var groupBorrowsByBookID = context.Borrows.GroupBy(x => x.Book);
            List<MyType>result = groupBorrowsByBookID.Select(y => 
            new MyType()
            {  
                MyString = y.Key.Title,
                MyDouble = y.Sum(x => ((TimeSpan)((x.ReturnedDate ?? DateTime.Today) - x.CheckedOutDate)).Days)               
            }).Cast<MyType>().ToList();
            return result;


            //var tempResult = group.Select(y => new MyType()
            //{
            //    MyString = y.Key.Name,
            //    MyDouble = y.Sum(x => x.Borrows.Sum(y => ((y.ReturnedDate ?? DateTime.Today.Date) - y.CheckedOutDate.Date).TotalDays))
            //}).ToList();
            //return tempResult;

            //var results = group.Select(y => new MyType()
            //{
            //    MyInt = y.Key,
            //    MyDouble = y.Sum(x => x.Borrows.Sum(y => ((y.ReturnedDate ?? DateTime.Today.Date) - y.CheckedOutDate.Date).TotalDays))
            //}).ToList();
            //var result = context.Books.Include(x => x.Borrows).Include(x => x.Author).Select()
        }
    }
}

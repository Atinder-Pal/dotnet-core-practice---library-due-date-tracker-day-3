using System;
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
            if(Request.Method =="POST")
            {
                ViewBag.status = filter == "true" ? "checked" : "";
                if(filter == "true")
                    ViewBag.list = GetOverdueBooks();
                else
                    ViewBag.list = GetBooks();
            }
            else
            {
                ViewBag.list = GetBooks();
            }            
            //ViewBag.overdueBooks = GetOverdueBooks();
            return View();
        }

        public IActionResult Details(string id)
        {
            try
            {
                ViewBag.bookDetails = GetBookByID(id);
            }
            catch (ValidationException e)
            {
                ViewBag.addMessage = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;

                //ViewBag.addMessage = TempData["Message"];
                //ViewBag.Exception = TempData["Exception"];
                //ViewBag.Error = TempData["Error"];
            }            
            return View();
        }

        public IActionResult Extend(string id)
        {
            ExtendDueDateForBookByID(id);
            return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        }

        public IActionResult Return(string id)
        {
            ReturnBookByID(id);
            return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        }

        public IActionResult Delete(string id)
        {
            DeleteBookByID(id);
            return RedirectToAction("List");
        }
        public IActionResult Borrow(string id)
        {
            try
            {
                CreateBorrow(id);
                ViewBag.addMessage = "Borrow successful!";
            }
            catch (ValidationException e)
            {                
                ViewBag.addMessage = "Borrow unsuccessful, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;

                //TempData["Message"] = ViewBag.addMessage;
                //TempData["Exception"] = ViewBag.Exception;
                //TempData["Error"] = ViewBag.Error;
            }
            return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        }

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
                if(!int.TryParse(id, out parsedID))
                {
                    exception.ValidationExceptions.Add(new Exception("Book ID not valid"));
                }
                else
                {
                    if(!context.Books.Any(book => book.ID == parsedID))
                    {
                        exception.ValidationExceptions.Add(new Exception("Book not found"));
                    }
                }
            }
            if(exception.ValidationExceptions.Count >0)
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
            using LibraryContext context = new LibraryContext();
            context.Books.Remove(context.Books.Where(book => book.ID == int.Parse(id)).SingleOrDefault());
            context.SaveChanges();
        }

        public List<Book> GetBooks()
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Include(book => book.Borrows).Include(x => x.Author).ToList();
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

            List<Book> overdueBooks1 = context.Books.Include(book => book.Borrows).Include(x => x.Author).Where(book => overdueBooks.Contains(book)).ToList();
            return overdueBooks1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDueDateTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryDueDateTracker.Controllers
{
    public class BorrowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public static void ExtendDueDateForBorrowByID(string bookID)
        {
            using LibraryContext context = new LibraryContext();
            var listRequiredBorrow = context.Borrows.Where(borrow => borrow.BookID == int.Parse(bookID)).ToList();
            Borrow requiredBorrow = listRequiredBorrow.LastOrDefault();
            requiredBorrow.DueDate = requiredBorrow.DueDate.AddDays(7);
            context.SaveChanges();
        }
        public static void ReturnBorrowByID(string id)
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
                        if(!book.Borrows.Any())
                        {
                            exception.ValidationExceptions.Add(new Exception("Book has never been checked out"));
                        }
                        //if book is not returned, it can't be borrowed
                        else if (!book.Borrows.Any(x => x.ReturnedDate == null))
                        {
                            exception.ValidationExceptions.Add(new Exception("Book has not been checked out after last return"));
                        }  
                        else if(book.Borrows.Where(x => x.ReturnedDate == null).SingleOrDefault().CheckedOutDate > DateTime.Today)
                        {
                            exception.ValidationExceptions.Add(new Exception("Return date can not be prior to CheckedOut Date"));
                        }                        
                    }
                }
            }
            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            
            //Citation
            //https://github.com/dotnet/efcore/issues/19583
            //Above source suggested to break my query into 2 steps- storing result of Where clause query in a var and then applying LastorDefault() on it as apparently LastOrDefault() don't work on Dbset for avoiding unauthorized accesses
            //Note: I was getting exception when trying to do in one go- that Linq expression can't be translated to query
            var listRequiredBorrow = context.Borrows.Where(borrow => borrow.BookID == parsedID).ToList();
            Borrow requiredBorrow = listRequiredBorrow.LastOrDefault();
            //End Citation
            requiredBorrow.ReturnedDate = DateTime.Today;
            context.SaveChanges();
        }

        public static void CreateBorrow(string id)
        {
            using LibraryContext context = new LibraryContext();

            int parsedID = 0;
            ValidationException exception = new ValidationException();
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
                        if(book.Borrows.Any())
                        {
                            //if book is not returned, it can't be borrowed
                            if (book.Borrows.Any(x => x.ReturnedDate == null))
                            {
                                exception.ValidationExceptions.Add(new Exception("Book already checked out, It can't be borrowed again without returning it."));
                            }
                        }                        
                    }
                }
            }
            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            Borrow newBorrow = new Borrow()
            {
                CheckedOutDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                ReturnedDate = null
            };
            newBorrow.BookID = parsedID;
            context.Borrows.Add(newBorrow);
            //context.Books.Where(book => book.ID == int.Parse(id)).Single().Borrows.Add(newBorrow);
            context.SaveChanges();
        }
    }
}

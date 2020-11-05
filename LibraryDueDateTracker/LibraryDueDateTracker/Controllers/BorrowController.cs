using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDueDateTracker.Models;
using Microsoft.AspNetCore.Mvc;

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
        public static void ReturnBorrowByID(string bookID)
        {
            using LibraryContext context = new LibraryContext();
            //Citation
            //https://github.com/dotnet/efcore/issues/19583
            //Above source suggested to break my query into 2 steps- storing result of Where clause query in a var and then applying LastorDefault() on it as apparently LastOrDefault() don't work on Dbset for avoiding unauthorized accesses
            //Note: I was getting exception when trying to do in one go- that Linq expression can't be translated to query
            var listRequiredBorrow = context.Borrows.Where(borrow => borrow.BookID == int.Parse(bookID)).ToList();
            Borrow requiredBorrow = listRequiredBorrow.LastOrDefault();
            //End Citation
            requiredBorrow.ReturnedDate = DateTime.Today;
            context.SaveChanges();
        }

        public static void CreateBorrow(string id)
        {
            using LibraryContext context = new LibraryContext();
            Borrow newBorrow = new Borrow()
            {
                CheckedOutDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                ReturnedDate = null
            };
            newBorrow.BookID = int.Parse(id);
            context.Borrows.Add(newBorrow);

            //context.Books.Where(book => book.ID == int.Parse(id)).Single().Borrows.Add(newBorrow);
            context.SaveChanges();
        }
    }
}

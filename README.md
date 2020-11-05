# dotnet-core-practice---library-due-date-tracker-day-3-Atinder-Pal
**Purpose:** This assignment is meant to challenge your mastery of ASP.NET Web Application (Model - View - Controller) and how well you are able to use MVC to create a CRUD application. Your goal in this assignment is to create a tool that will help you keep track of all the books you have checked out of the library. This is a cumulative activity. Use your code from ASP.NET Core Assignment - Library Due Date Tracker Day 2 as a starting point.

**Author:** Atinder Pal

**Requirements:**
* Modify “Borrow” (Model):
  * Add a property “ExtensionCount” - int(10), not nullable.
    * Update your seed data for this table to include values for this field.
  * Add a migration.
  * Update the database.
* Modify “List” (View / Action):
  * Create a form with a checkbox “Filter to Overdue”.
    * When the page loads with the checkbox checked (query string parameter), call the “GetOverdueBooks()” method instead of the “GetBooks()” method.
* Modify “Details” (View / Action):
  * Add a “Number of Extensions” line / output.
* Add the following business logic to the controllers:
  * General Validation:
    * Trimmed all data prior to processing.
    * All comparison validation must be case insensitive.
    * String data cannot exceed its database size.
    * NOT NULL fields must have values that are not whitespace.
    * All numeric/date fields must successfully parse.
    * Primary keys on ‘ByID’ methods must exist.
    * Reference IDs (foreign keys) must exist in their respective tables.
* Library Business Logic:
  * “CheckedOutDate” cannot be prior to “PublicationDate”.
  * “ReturnedDate” cannot be prior to “CheckedOutDate”.
  * “PublishedDate” cannot be in the future.
  * An extension must actually extend the due date in order to be valid.
  * Overdue books cannot be extended.
  * Books cannot be extended more than 3 times.
  * Book titles must be unique for that author.
* Display itemized errors on all appropriate “Book” view pages.

**Challenges:**
* Make it look nice with CSS.
* Modify “List” (View) to show the user how many days a book is overdue, and make the text dark red.
* Add an “Archived” flag to “Book” that will become the new method for “DeleteBookByID”.
  * Set the flag to true when a book is deleted.
  * Don’t allow a book that isn’t returned to be archived.
  * Don’t show the book on “List” unless archived books are being shown.
  * Don’t allow any borrows to take place for the book.
* Modify the checkbox on “List” (View) to be a dropdown with multiple types of filters:
  * All Books
  * In-Stock Books
  * Lent Books
  * Overdue Books (Included in Lent Books)
  * Archived Books
* Create a “Report” Action / View that will provide some summaries about the data.
  * Determine which author’s books have the longest total checked-out time.
    * This should work with books that haven’t been returned, as well as on books that have been returned.
* Have an unexpected feature.

**Link to Trello Board:** https://trello.com/b/MFCGUDai/library-due-date-tracker-day-3

using System;

class Program
{
    static void Main(string[] args)
    {
        Library lib = new Library(maxBooks: 10, maxBorrowers: 10, maxEvents: 20, tableCapacity: 10);

        Console.WriteLine("=== Adding Books ===");
        Console.WriteLine(lib.AddBook("100.00", "Philosophy"));     // true
        Console.WriteLine(lib.AddBook("567.45", "Life Sciences"));  // true
        Console.WriteLine(lib.AddBook("005.10", "Programming"));    // true
        Console.WriteLine(lib.AddBook("567.45", "Duplicate"));      // false

        Console.WriteLine("\n=== Adding Borrowers ===");
        Console.WriteLine(lib.AddBorrower("n12345678", "Alice")); // true
        Console.WriteLine(lib.AddBorrower("n87654321", "Bob"));   // true
        Console.WriteLine(lib.AddBorrower("n12345678", "Again")); // false

        Console.WriteLine("\n=== ContainsBook ===");
        Console.WriteLine(lib.ContainsBook("567.45")); // true
        Console.WriteLine(lib.ContainsBook("999.99")); // false

        Console.WriteLine("\n=== Record Borrow Events ===");
        Console.WriteLine(lib.RecordBorrowEvent("n12345678", "567.45")); // true
        Console.WriteLine(lib.RecordBorrowEvent("n12345678", "567.45")); // true
        Console.WriteLine(lib.RecordBorrowEvent("n12345678", "567.45")); // true (violator)
        Console.WriteLine(lib.RecordBorrowEvent("n87654321", "005.10")); // true
        Console.WriteLine(lib.RecordBorrowEvent("badID", "567.45"));     // false
        Console.WriteLine(lib.RecordBorrowEvent("n12345678", "000.00")); // false

        Console.WriteLine("\n=== Find Borrow Limit Violator ===");
        Console.WriteLine(lib.FindBorrowLimitViolator("567.45")); // should be n12345678
        Console.WriteLine(lib.FindBorrowLimitViolator("005.10")); // likely null

        Console.WriteLine("\n=== Most Borrowed Subject ===");
        Console.WriteLine(lib.MostBorrowedSubject()); // should be "567.45"

        Console.WriteLine("\n=== GetBookIndex (before sorting) ===");
        try
        {
            Console.WriteLine(lib.GetBookIndex("567.45"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        Console.WriteLine("\n=== Sort Catalogue ===");
        try
        {
            lib.SortCatalogue();
            Console.WriteLine("Catalogue sorted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        Console.WriteLine("\n=== GetBookIndex (after sorting) ===");
        try
        {
            Console.WriteLine(lib.GetBookIndex("100.00"));
            Console.WriteLine(lib.GetBookIndex("567.45"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        Console.WriteLine("\n=== Done ===");
    }
}
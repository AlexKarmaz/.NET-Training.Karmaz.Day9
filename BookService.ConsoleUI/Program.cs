using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookService;
using System.IO;

namespace BookService.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            BookListService service = new BookListService(new BookFileRepository("TestBooksFile"));

            if (!File.Exists("TestBooksFile"))
                service.AddBooks(new List<Book> {
                    new Book("J. K. Rowling", "Harry Potter and the Prisoner of Azkaban", 528, 18.4),
                    new Book("Stephen King", "The Green Mile", 384, 10.6),
                    new Book("Борис Васильев", "А зори здесь тихие...", 432, 10.2),
                    new Book("Антуан де Сент-Экзюпери", "Маленький принц", 104, 16.7),
                });

            IEnumerable<Book> sort = service.SortBooksByTag(book => book.Price);
            Console.WriteLine("Sort by price: ");
            foreach (var book in sort)
            {
                Console.WriteLine($"{book}");
            }

            sort = service.SortBooksByTag(book => book.NumberPages);
            Console.WriteLine("Sort by number of pages: ");
            foreach (var book in sort)
            {
                Console.WriteLine($"{book}");
            }

            IEnumerable<Book> find = service.FindBookByTag(book => book.Author == "Антуан де Сент-Экзюпери");
            Console.WriteLine("Find by author Антуан де Сент-Экзюпери");
            foreach (var book in find)
            {
                Console.WriteLine($"{book} ");
            }

            Console.WriteLine("Test remove (Harry Potter) and add (Godfather):");
            service.AddBook(new Book("Mario Puzo", "Godfather", 288, 25.6));
            service.RemoveBook(new Book("J. K. Rowling", "Harry Potter and the Prisoner of Azkaban", 528, 18.4));

            foreach (var book in service.Books)
            {
                Console.WriteLine($"{book}");
            }

            Console.WriteLine("Test exceptions:");
            try
            {
                service.AddBook(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                service.RemoveBook(new Book("Margaret Mitchell", "Gone with the Wind", 1280, 17));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}

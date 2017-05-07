using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookService.ConsoleUI
{
    class BookXMLRepository : IRepository<Book>
    {
        private string Path { get; }


        public BookXMLRepository(string filePath)
        {
            Path = filePath;
        }

        #region Public Methods
        public IEnumerable<Book> GetAllItems()
        {
            List<Book> books = new List<Book>();
            XDocument xdoc = XDocument.Load(Path);

            foreach (var item in xdoc.Element("BookList").Elements("Book"))
            {
                XElement author = item.Element("Author");
                XElement title = item.Element("Title");
                XElement numberPages = item.Element("NumberPages");
                XElement price = item.Element("Price");
                books.Add(new Book(author.Value, title.Value, int.Parse(numberPages.Value), int.Parse(price.Value)));
            }
            return books;
        }

        public void Create(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            WriteBook(new List<Book> { item }, FileMode.Append);
        }

        public void Create(IEnumerable<Book> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            WriteBook(items, FileMode.Append);
        }

        public bool Delete(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            List<Book> books = GetAllItems().ToList();
            bool result = books.Remove(item);
            if (result)
                WriteBook(books, FileMode.Create);
            return result;
        }
        #endregion

        #region Private Method
        private void WriteBook(IEnumerable<Book> items, FileMode mode)
        {
            XDocument document = new XDocument();
            document.Add(new XElement("BookList"));

            foreach (var book in items)
            {
                XElement node = new XElement("Book");
                XElement author = new XElement("Author", book.Author);
                XElement title = new XElement("Title", book.Title);
                XElement numberPages = new XElement("NumberPages", book.NumberPages);
                XElement price = new XElement("Price", book.Price);

                node.Add(author, title, numberPages, price);
                document.Root.Add(node);
            }
            document.Save(Path);
        }
        #endregion 
    }
}

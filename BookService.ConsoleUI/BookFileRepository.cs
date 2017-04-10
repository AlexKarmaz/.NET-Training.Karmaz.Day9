using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.ConsoleUI
{
    class BookFileRepository: IRepository<Book>
    {
        private string Path { get; }
       

        public BookFileRepository(string filePath)
        {
            Path = filePath;
        }

        #region Public Methods
        public IEnumerable<Book> GetAllItems()
        {
            FileMode mode;
            if (File.Exists(Path)) mode = FileMode.Open;
            else mode = FileMode.Create;

            List <Book> books = new List<Book>();
            using (BinaryReader reader = new BinaryReader(new FileStream(Path, mode)))
            {
                while (reader.PeekChar() > -1)
                {
                    string author = reader.ReadString();
                    string title = reader.ReadString();
                    int pagesNumber = reader.ReadInt32();
                    double price = reader.ReadDouble();
                    books.Add(new Book(author, title, pagesNumber, price));

                }
            }
            return books;
        }

        public void Create(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            WriteBook(new List<Book> { item } , FileMode.Append);
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
            using (BinaryWriter writer = new BinaryWriter(new FileStream(Path, mode, FileAccess.Write)))
            {
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        writer.Write(item.Author);
                        writer.Write(item.Title);
                        writer.Write(item.NumberPages);
                        writer.Write(item.Price);
                    }
                }
            }
        }
        #endregion 

    }
}

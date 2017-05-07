using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BookService.ConsoleUI
{
    class BookSerializedRepository : IRepository<Book>
    {
        private string Path { get; }


        public BookSerializedRepository(string filePath)
        {
            Path = filePath;
        }

        #region Public Methods
        public IEnumerable<Book> GetAllItems()
        {
            List<Book> books = new List<Book>();
            BinaryFormatter formatter = new BinaryFormatter();
            if (!File.Exists(Path))
                throw new FileNotFoundException($"File \"{Path}\" not found.");

            using (FileStream fs = new FileStream(Path, FileMode.Open))
            {
                books = (List<Book>)formatter.Deserialize(fs);
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
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(Path, mode, FileAccess.Write))
            {
                formatter.Serialize(fs, items);
            }
        }
        #endregion 

    }
}

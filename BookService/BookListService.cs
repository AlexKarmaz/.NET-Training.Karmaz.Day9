using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService
{
    public class BookListService
    {
        private IRepository<Book> Repository { get; }
        public IEnumerable<Book> Books => Repository.GetAllItems();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public BookListService(IRepository<Book> repository)
        {
            Repository = repository;
        }

        #region Public methods
        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            if (Repository.GetAllItems().Contains(book))
            {
                logger.Trace($"BookListService contains {book}");
            }
            Repository.Create(book);
        }

        public void AddBooks(IEnumerable<Book> books)
        {
            if (books == null)
                throw new ArgumentNullException(nameof(books));
            var booksCollection = books as IList<Book> ?? books.ToList();
            foreach (var book in booksCollection.Where(book => Repository.GetAllItems().Contains(book)))
            {
                logger.Trace($"BookListService contains {book}");
            }
            Repository.Create(booksCollection);
        }

        public void RemoveBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            if (!Repository.Delete(book))
            {
                logger.Info($"BookListService doesn't contain {book}");
                throw new ArgumentException($"BookListService doesn't contain {book}");
            }
            else
            {
                logger.Debug($"The book {book} was deleted");
            }
        }

        public IEnumerable<Book> SortBooksByTag(Func<Book, object> keySelector) => Repository.GetAllItems().OrderBy(keySelector);

        public IEnumerable<Book> FindBookByTag(Predicate<Book> match) => Repository.GetAllItems().ToList().FindAll(match);
        #endregion
    }
}

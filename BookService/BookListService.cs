using BookService;
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
        /// <summary>A repository object that provides an implementation of the IRepository interface</summary>
        private IRepository<Book> Repository { get; }
        /// <summary>Get all books from repository</summary>
        public IEnumerable<Book> Books => Repository.GetAllItems();

        private readonly ILogger logger;

        public BookListService(IRepository<Book> repository, ILogger logger)
        {
            Repository = repository;

            if (ReferenceEquals(logger, null))
                logger = NLogProvider.GetLogger(nameof(BookListService));

            logger.Debug("Constructor {0} with two parameters is started.", nameof(BookListService));
        }

        public BookListService(ILogger logger)
        {
            if (ReferenceEquals(logger, null))
                logger = NLogProvider.GetLogger(nameof(BookListService));

            logger.Debug("Constructor {0} with one parameter is started.", nameof(BookListService));
            this.logger = logger;
        }
        #region Public methods
        /// <summary>Add book to repository</summary>
        /// <param name="book">Book to add</param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>Add books to repository</summary>
        /// <param name="books">Books to add</param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>Remove book from repository</summary>
        /// <param name="book">Book for removal</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>Sorts the elements in ascending order according to a key</summary>
        /// <param name="keySelector">A function to extract a key from an element</param>
        /// <returns>A <see cref="IEnumerable{Book}"/> whose elements are sorted according to a key</returns>
        public IEnumerable<Book> SortBooksByTag(Func<Book, object> keySelector) => Repository.GetAllItems().OrderBy(keySelector);
        /// <summary>Retrieves all the element that match the conditions by the specified predicate</summary>
        /// <param name="match">The <see cref="Predicate{Book}"/> delegate that defines the conditions of the elements to search for</param>
        /// <returns>A <see cref="IEnumerable{Book}"/> containing all the elements that match the conditions defined by the specified predicate, if found</returns>
        public IEnumerable<Book> FindBookByTag(Predicate<Book> match) => Repository.GetAllItems().ToList().FindAll(match);
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService
{
    public class Book : IEquatable<Book>, IComparable<Book>
    {
        #region Public Properties 
        public string Author { get; }
        public string Title { get; }
        public int NumberPages { get; }
        public double Price { get; }
        #endregion

        public Book(string author, string title, int numberPages, double price)
        {
            Author = author;
            Title = title;
            NumberPages = numberPages;
            Price = price;
        }

        #region Public Methods 
        public bool Equals(Book other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Author, other.Author) && string.Equals(Title, other.Title) && NumberPages == other.NumberPages && Price.Equals(other.Price);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            else return Equals((Book)obj);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            unchecked
            {
                hash = (hash * 7) + Author.GetHashCode();
                hash = (hash * 7) + Title.GetHashCode();
                hash = (hash * 7) + NumberPages.GetHashCode();
                hash = (hash * 7) + Price.GetHashCode();
                return hash;
            }
            
        }

        public int CompareTo(Book other)
        {
            if (other == null) return 1;
            return Author.CompareTo(other.Author);
        }

        public override string ToString() => $"Author: {Author}, Title: {Title}, PagesNumber: {NumberPages}, Price: {Price}";
        #endregion
    }
}

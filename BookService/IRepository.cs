using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAllItems();
        void Create(T item);
        void Create(IEnumerable<T> items);
        bool Delete(T item);
    }
}

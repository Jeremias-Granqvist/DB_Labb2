using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model
{
    public class BookManager : ModelBase
    {
        public event EventHandler<Book> BookAdded;

        public void AddBook(Book book)
        {
            using (var context = new BookstoreContext())
            {
                context.Add(book);
                context.SaveChanges();

            }
            OnBookAdded(book);
        }
        protected virtual void OnBookAdded(Book book)
        {
            BookAdded?.Invoke(this, book);
        }
    }
}

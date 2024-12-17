using Microsoft.EntityFrameworkCore;
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
        public event EventHandler<Book> BookEdited;
        public event EventHandler<long> BookDeleted;
        public void AddBook(Book book)
        {
            using (var context = new BookstoreContext())
            {
                context.Books.Add(book);
                foreach (var author in book.Authors)
                {
                    context.BookAuthors.Add(new BookAuthor
                    {
                        ISBN13 = book.ISBN13,
                        AuthorID = author.AuthorID
                    });
                }
                context.SaveChanges();
            }
            OnBookAdded(book);
        }
        public void EditBook(Book updatedBook, long originalISBN)
        {
            using (var context = new BookstoreContext())
            {
                    
                var existingBook = context.Books.Include(b => b.Authors)
                    .FirstOrDefault(b => b.ISBN13 == updatedBook.ISBN13);
                
                if (existingBook != null)
                {
                    existingBook.ISBN13 = updatedBook.ISBN13;
                    existingBook.Title = updatedBook.Title;
                    existingBook.ReleaseDate = updatedBook.ReleaseDate;
                    existingBook.Price = updatedBook.Price;
                    existingBook.Language = updatedBook.Language;

                    existingBook.Authors.Clear();
                    foreach (var author in updatedBook.Authors)
                    {
                        var existingAuthor = context.Authors.Local.FirstOrDefault(a => a.AuthorID == author.AuthorID);
                        if (existingAuthor == null)
                        {
                            context.Authors.Attach(author);
                            existingBook.Authors.Add(author);
                            context.BookAuthors.Add(new BookAuthor
                            {
                                ISBN13 = existingBook.ISBN13,
                                AuthorID = author.AuthorID
                            });
                        }
                        else
                        {
                            existingBook.Authors.Add(existingAuthor);
                        }
                    }

                    context.SaveChanges();
                }
                else if (updatedBook.ISBN13 == originalISBN)
                {
                    existingBook = context.Books.Include(b => b.Authors)
                        .FirstOrDefault(b => b.ISBN13 == originalISBN);

                    originalISBN = updatedBook.ISBN13;
                    existingBook.Title = updatedBook.Title;
                    existingBook.ReleaseDate = updatedBook.ReleaseDate;
                    existingBook.Price = updatedBook.Price;
                    existingBook.Language = updatedBook.Language;

                    existingBook.Authors.Clear();
                    foreach (var author in updatedBook.Authors)
                    {
                        var existingAuthor = context.Authors.Local.FirstOrDefault(a => a.AuthorID == author.AuthorID);
                        if (existingAuthor == null)
                        {
                            context.Authors.Attach(author);
                            existingBook.Authors.Add(author);
                        }
                        else
                        {
                            existingBook.Authors.Add(existingAuthor);
                        }
                    }
                    context.SaveChanges();
                    RaisePropertyChanged("Authors");
                    RaisePropertyChanged();
                    RaisePropertyChanged("BookAuthor");
                }

            }
            OnBookEdited(updatedBook);
        }
        public void DeleteBook(long ISBN)
        {
            using (var context = new BookstoreContext())
            {
                var bookToDelete = context.Books.FirstOrDefault(b => b.ISBN13 == ISBN);
                if (bookToDelete != null)
                {
                    context.Remove(bookToDelete);
                    context.SaveChanges();
                }
            }
            OnBookDeleted(ISBN);
        }
        protected virtual void OnBookAdded(Book book)
        {
            BookAdded?.Invoke(this, book);
        }
        protected virtual void OnBookEdited(Book book)
        {
            BookEdited?.Invoke(this, book);
        }
        protected virtual void OnBookDeleted(long isbn)
        {
            BookDeleted?.Invoke(this, isbn);
        }
    }
}

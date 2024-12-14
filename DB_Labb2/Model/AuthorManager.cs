using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model
{
   public class AuthorManager : ModelBase
    {
        public event EventHandler<Author> AuthorAdded;
        public event EventHandler<Author> AuthorEdited;
        public event EventHandler<int> AuthorDeleted;
        public void AddAuthor(Author author)
        {
            using (var context = new BookstoreContext())
            {
                context.Add(author);
                context.SaveChanges();

            }
            OnAuthorAdded(author);
        }

        public void EditAuthor(Author updatedAuthor)
        {
            using (var context = new BookstoreContext())
            {
                var existingAuthor = context.Authors.FirstOrDefault(a => a.AuthorID == updatedAuthor.AuthorID);
                if (existingAuthor != null)
                {
                    existingAuthor.Firstname = updatedAuthor.Firstname;
                    existingAuthor.Lastname = updatedAuthor.Lastname;
                    existingAuthor.Birthdate = updatedAuthor.Birthdate;
                    context.SaveChanges();
                }
            }
            OnAuthorEdited(updatedAuthor);
        }

        public void DeleteAuthor(int authorID)
        {
            using (var context = new BookstoreContext())
            {
                var authorToDelete = context.Authors.FirstOrDefault(a => a.AuthorID == authorID);
                if (authorToDelete != null)
                {
                    context.Remove(authorToDelete);
                    context.SaveChanges();
                }
            }
            OnAuthorDeleted(authorID);
        }

        protected virtual void OnAuthorAdded(Author author)
        {
            AuthorAdded?.Invoke(this, author);
        }
        protected virtual void OnAuthorEdited(Author author)
        {
            AuthorEdited?.Invoke(this, author);
        }
        protected virtual void OnAuthorDeleted(int authorID)
        {
            AuthorDeleted?.Invoke(this, authorID);
        }
    }
}

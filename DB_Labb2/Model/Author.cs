using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model;

public class Author
{
    public int AuthorID { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public DateTime Birthdate { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();

}


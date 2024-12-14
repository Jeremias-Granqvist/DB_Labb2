using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model;

public class Author : ModelBase
{
    public int AuthorID { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public DateOnly Birthdate { get; set; }

    public string FullName => $"{Firstname} {Lastname}";
    public ICollection<Book> Books { get; set; } = new List<Book>();

}


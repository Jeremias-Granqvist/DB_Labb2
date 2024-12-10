using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model;
public class Book
{
    public long ISBN13 { get; set; }
    public string? Title { get; set; }
    public int Price { get; set; }
    public string? Language { get; set; }

    [Column("Release Date")]
    public DateTime ReleaseDate { get; set; }

    public ICollection<Author> Authors { get; set; } = new List<Author>();


}

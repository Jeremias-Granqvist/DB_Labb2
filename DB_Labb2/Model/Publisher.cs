using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        [Column("Publisher Name")]
        public string PublisherName { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}

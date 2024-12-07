using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model
{
    public class Format
    {
        public int FormatID { get; }
        public string FormatTyp { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();


    }
}

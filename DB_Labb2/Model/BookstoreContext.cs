using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Labb2.Model;

public class BookstoreContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new SqlConnectionStringBuilder()
        {
            ServerSPN = "localhost",
            InitialCatalog = "Jeremias Granqvist Labb1",
            TrustServerCertificate = true,
            IntegratedSecurity = true
        }.ToString();

        optionsBuilder.UseSqlServer(connectionString);
    }
}

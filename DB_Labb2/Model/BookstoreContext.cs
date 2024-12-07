using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace DB_Labb2.Model;

public partial class BookstoreContext : DbContext
{

    public BookstoreContext()
    {
    }
    public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; } 
    public virtual DbSet<Format> Formats { get; set; }
    public virtual DbSet<Publisher> Publishers { get; set; }


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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Finnish_Swedish_CI_AS");

        //modelBuilder.Entity<Book>()
        //        .HasMany(b => b.Authors)
        //        .WithMany(a => a.Books)
        //        .UsingEntity<BookAuthor>(
        //            j => j
        //                .HasOne(ba => ba.Author)
        //                .WithMany()
        //                .HasForeignKey(ba => ba.AuthorID),
        //            j => j
        //                .HasOne(ba => ba.Book)
        //                .WithMany()
        //                .HasForeignKey(ba => ba.ISBN13),
        //            j =>
        //            {
        //                j.HasKey(ba => new { ba.ISBN13, ba.AuthorID });
        //                j.ToTable("BookAuthor");
        //            }
        //        );


        modelBuilder.Entity<Book>()
                    .HasKey(b => b.ISBN13);

        modelBuilder.Entity<Book>()
                    .HasOne(b => b.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(b => b.PublisherID);

        modelBuilder.Entity<Book>()
                    .HasOne(b => b.Format)
                    .WithMany(f => f.Books)
                    .HasForeignKey(b => b.FormatID);

        modelBuilder.Entity<Book>()
                    .Property(b => b.Title)
                    .HasMaxLength(255);
       
        modelBuilder.Entity<Book>()
                    .Property(b => b.Language)
                    .HasMaxLength(50);

        modelBuilder.Entity<Book>()
                    .Property(b => b.ReleaseDate)
                    .HasConversion(
                        v => v.Date,
                        v => v
                        );


        modelBuilder.Entity<Author>()
                    .Property(a => a.Birthdate)
                    .HasConversion(
                        v => v.Date,
                        v => v
                        );
        modelBuilder.Entity<Author>()
                    .HasKey(a => a.AuthorID);

        modelBuilder.Entity<Format>()
                    .HasKey(f => f.FormatID);

        modelBuilder.Entity<Publisher>()
                    .HasKey(p => p.PublisherID);

        base.OnModelCreating(modelBuilder);

    }
}

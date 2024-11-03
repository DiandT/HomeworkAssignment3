using System.Data.Entity;
using HomeworkAssignment3.Models; // Replace with the actual namespace for your models

public class LibraryContext : DbContext
{
    public LibraryContext() : base("name=LibraryDbConnectionString")
    {
  
    }

    // DbSets for each table
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Type> Types { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships based on your ER diagram
        modelBuilder.Entity<Book>()
            .HasRequired(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<Book>()
            .HasRequired(b => b.Type)
            .WithMany(t => t.Books)
            .HasForeignKey(b => b.TypeId)
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<Borrow>()
            .HasRequired(br => br.Book)
            .WithMany(b => b.Borrows)
            .HasForeignKey(br => br.BookId)
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<Borrow>()
            .HasRequired(br => br.Student)
            .WithMany(s => s.Borrows)
            .HasForeignKey(br => br.StudentId)
            .WillCascadeOnDelete(false);
    }
}

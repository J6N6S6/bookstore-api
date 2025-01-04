using Microsoft.EntityFrameworkCore;
using Bookstore.Models;

namespace Bookstore.Data
{
    public class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Bookstores> Bookstores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bookstores>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(b => b.Title).IsUnique();
                entity.Property(b => b.Title).IsRequired();
                entity.Property(b => b.Author).IsRequired();
                entity.Property(b => b.Price).IsRequired();

                entity.HasOne<Bookstores>()
                    .WithMany()
                    .HasForeignKey(b => b.BookstoreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }

    }
}

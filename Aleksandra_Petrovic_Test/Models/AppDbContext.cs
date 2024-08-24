using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aleksandra_Petrovic_Test.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ComicBook> ComicBooks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

                 modelBuilder.Entity<ComicBook>(entity =>
        {
            entity.Property(e => e.Price)
                  .HasPrecision(18, 2);
             });


            modelBuilder.Entity<Publisher>().HasData(
                  new Publisher() { Id = 1, Name = "Max", Year = 1994 },
                    new Publisher() { Id = 2, Name = "Blue", Year = 2005 },
                      new Publisher() { Id = 3, Name = "Kreativno", Year = 1994 }
                  );

            modelBuilder.Entity<ComicBook>().HasData(
                new ComicBook() { Id = 1, Genre = "Herojski", Name = "Rista brzi", Price = 400m, AvailableQuantity = 20, PublisherId = 3 },
                  new ComicBook() { Id = 2, Genre = "Manga", Name = "Dragon run", Price = 880m, AvailableQuantity = 210, PublisherId = 1 },
                    new ComicBook() { Id = 3, Genre = "Horor", Name = "Tamni put", Price = 1400m, AvailableQuantity = 10, PublisherId = 2 },
                      new ComicBook() { Id = 4, Genre = "Manga", Name = "Rising hero", Price = 1340m, AvailableQuantity = 30, PublisherId = 3 },
                        new ComicBook() { Id = 5, Genre = "Herojski", Name = "Babo max", Price = 540m, AvailableQuantity = 70, PublisherId = 1 }



                );


            base.OnModelCreating(modelBuilder);

        }
    }
}
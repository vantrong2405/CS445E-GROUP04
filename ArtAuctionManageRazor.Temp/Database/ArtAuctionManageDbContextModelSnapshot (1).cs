using ArtAuctionManageRazor.Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtAuctionManageRazor.Temp.Database
{
    public class ArtAuctionManageRazorDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public ArtAuctionManageRazorDbContext(DbContextOptions<ArtAuctionManageRazorDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed catogories data
            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category()
                {
                    Id = 2,
                    Name = "Scifi",
                    DisplayOrder = 2
                },
                new Category()
                {
                    Id = 3,
                    Name = "History",
                    DisplayOrder = 3
                },
            };
            foreach (var category in categories)
            {
                modelBuilder.Entity<Category>().HasData(category);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

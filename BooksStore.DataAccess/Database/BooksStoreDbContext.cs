using BooksStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace BooksStore.DataAccess.Database
{
    public class BooksStoreDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public BooksStoreDbContext(DbContextOptions<BooksStoreDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data for category table
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

            //seed data for products table
            var stringBuilder = new StringBuilder();
            using (var streamReader = new StreamReader(@"D:\\aspnetcore\\BooksStoreSolution\\BooksStore.Web\\products.json"))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    stringBuilder.AppendLine(line);
                }
            };
            var products = JsonSerializer.Deserialize<List<Product>>(stringBuilder.ToString());
            if (products != null)
            {
                foreach (var product in products)
                {
                    modelBuilder.Entity<Product>().HasData(product);
                }
            }

            //Seed data for companies table
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Tech Solution",
                    StreetAddress = "123 Tech St",
                    City = "Tech City",
                    PostalCode = "48291",
                    State = "IL",
                    PhoneNumber = "0938273645"
                },
                new Company
                {
                    Id = 2,
                    Name = "Vivid Books",
                    StreetAddress = "999 Vid St",
                    City = "Vid City",
                    PostalCode = "23958",
                    State = "IL",
                    PhoneNumber = "0965128390"
                },
                new Company
                {
                    Id = 3,
                    Name = "Readers Club",
                    StreetAddress = "999 Main St",
                    City = "Lala land",
                    PostalCode = "71924",
                    State = "NY",
                    PhoneNumber = "0917465283"
                });

            //Add foreign key
            modelBuilder.Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .HasPrincipalKey(c => c.Id);
        }
    }
}

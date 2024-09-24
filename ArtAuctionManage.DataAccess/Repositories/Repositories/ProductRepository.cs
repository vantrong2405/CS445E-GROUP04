using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BooksStoreDbContext db) : base(db) 
        {
        }

        public async Task Update(Product product)
        {
            var productEntity = await base.DbSet.FirstOrDefaultAsync(p => p.Id == product.Id);

            if (productEntity != null)
            {
                productEntity.Title = product.Title;
                productEntity.ISBN = product.ISBN;
                productEntity.Price = product.Price;
                productEntity.Price50 = product.Price50;
                productEntity.ListPrice = product.ListPrice;
                productEntity.Price100 = product.Price100;
                productEntity.Description = product.Description;
                productEntity.CategoryId = product.CategoryId;
                productEntity.Author = product.Author;
                if(product.ImageUrl != null)
                {
                    productEntity.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}

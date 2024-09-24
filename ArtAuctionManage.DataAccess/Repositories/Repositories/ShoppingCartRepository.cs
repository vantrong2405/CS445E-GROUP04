using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(BooksStoreDbContext db) : base(db) 
        {
        }

        public async Task Update(ShoppingCart shoppingCart)
        {
            this.DbSet.Update(shoppingCart);
            await this._db.SaveChangesAsync();
        }
    }
}

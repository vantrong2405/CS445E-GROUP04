using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BooksStoreDbContext _db;
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }

		public IOrderHeaderRepository OrderHeaders { get; private set; }

		public IOrderDetailRepository OrderDetails {  get; private set; }

		public UnitOfWork(BooksStoreDbContext db)
        {
            this._db = db;
            this.Categories = new CategoryRepository(db);
            this.Products = new ProductRepository(db);
            this.Companies = new CompanyRepository(db);
            this.ShoppingCarts = new ShoppingCartRepository(db);
            this.ApplicationUsers = new ApplicationUserRepository(db);
            this.OrderHeaders = new OrderHeaderRepository(db);
            this.OrderDetails = new OrderDetailRepository(db);
        }

        public async Task Save()
        {
            await this._db.SaveChangesAsync();
        }
    }
}

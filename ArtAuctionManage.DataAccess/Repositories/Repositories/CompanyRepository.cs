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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(BooksStoreDbContext db) : base(db)
        {

        }
        public Task Save()
        {
            throw new NotImplementedException();
        }

        public async Task Update(Company company)
        {
            this.DbSet.Update(company);
            await this._db.SaveChangesAsync();
        }
    }
}

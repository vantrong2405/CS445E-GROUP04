using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        Task<T?> GetDetails(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);

        Task<T> Add(T entity);

        Task<bool> Remove(T entity);

        Task<bool> RemoveRange(IEnumerable<T> entities);
    }
}

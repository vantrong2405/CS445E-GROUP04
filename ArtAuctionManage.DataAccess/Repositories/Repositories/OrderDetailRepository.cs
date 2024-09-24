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
	public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
		public OrderDetailRepository(BooksStoreDbContext db) : base(db)
		{

		}
		public Task Update(OrderDetail orderDetail)
		{
			throw new NotImplementedException();
		}
	}
}

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
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		public OrderHeaderRepository(BooksStoreDbContext db) : base(db)
		{

		}
		public async Task Update(OrderHeader orderHeader)
		{
			this.DbSet.Update(orderHeader);
			await this._db.SaveChangesAsync();
		}

        public async Task UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
			var orderHeader = await base._db.OrderHeaders.FirstOrDefaultAsync
				(o => o.Id == id);
			if (orderHeader != null)
			{
				orderHeader.OrderStatus = orderStatus;
				if (!string.IsNullOrEmpty(paymentStatus))
				{
					orderHeader.PaymentStatus = paymentStatus;
				}
			}
        }
    }
}

using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories.IRepositories
{
	public interface IOrderHeaderRepository : IRepository<OrderHeader>
	{
		Task Update(OrderHeader orderHeader);

        Task UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    }
}

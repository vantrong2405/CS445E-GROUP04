﻿using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task Update(ShoppingCart shoppingCart);
    }
}

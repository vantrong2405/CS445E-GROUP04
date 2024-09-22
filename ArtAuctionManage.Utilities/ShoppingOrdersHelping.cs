using ArtAuctionManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAuctionManage.Utilities
{
    public class ShoppingOrdersHelping
    {
        public static int GetTotalOrders(IEnumerable<ShoppingCart> shoppingCarts)
        {
            int count = 0;
            foreach (var item in shoppingCarts)
            {
                count++;
            }
            return count;
        }
    }
}

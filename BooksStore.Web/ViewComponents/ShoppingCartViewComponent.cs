using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Utilities;
using BooksStore.Web.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksStore.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private ILogger<ShoppingCartViewComponent> _logger;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork, ILogger<ShoppingCartViewComponent> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} view component",
                nameof(ShoppingCartViewComponent), nameof(this.InvokeAsync));

            var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var shoppingCarts = await this._unitOfWork.ShoppingCarts.GetAll
                    (s => s.ApplicationUserId == userId);

                if(this.HttpContext.Session.GetInt32(StaticDetails.SessionCart) == null)
                {
                    this.HttpContext.Session.SetInt32(StaticDetails.SessionCart,
                        ShoppingOrdersHelping.GetTotalOrders(shoppingCarts));
                }

                return this.View(this.HttpContext.Session.GetInt32(StaticDetails.SessionCart));
            }
            else
            {
                this.HttpContext.Session.Clear();

                return this.View(0);
            }
        }
    }
}

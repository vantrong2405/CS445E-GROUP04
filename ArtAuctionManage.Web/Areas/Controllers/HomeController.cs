using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using BooksStore.Models.DTOs;
using BooksStore.Utilities;
using BooksStore.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BooksStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
        }

        private ProductResponse ConvertProductToProductResponse(Product product)
        {
            var productResponse = product.ToProductResponse();
            productResponse.CategoryName = product?.Category?.Name;

            return productResponse;
        }

        [HttpGet]
        [Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(HomeController), nameof(this.Index));

            var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                this.HttpContext.Session.SetInt32(StaticDetails.SessionCart, 0);
            } else
            {
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var shoppingCarts = await this._unitOfWork.ShoppingCarts.GetAll
                    (s => s.ApplicationUserId == userId);
                
                this.HttpContext.Session.SetInt32(StaticDetails.SessionCart, 
                    ShoppingOrdersHelping.GetTotalOrders(shoppingCarts));
            }

            var products = await this._unitOfWork.Products.GetAll(includeProperties: "Category"); 

            var productsResponse = products.Select(p => this.ConvertProductToProductResponse(p)).ToList();

            return View(productsResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ProductDetails([FromQuery]int? productId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(HomeController), nameof(this.ProductDetails));

            if (productId == null)
            {
                return this.NotFound();
            }

            var product = await this._unitOfWork.Products.GetDetails
                ((p) => p.Id == productId, includeProperties: "Category");

            if(product == null)
            {
                return this.NotFound();
            }

            var shoppingCart = new ShoppingCart()
            {
                Count = 1,
                Product = product,
                ProductId = (int)productId
            };

            return this.View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> ProductDetails([FromForm]ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            shoppingCart.ApplicationUserId = userId;

            var shoppingCartFromDb = await this._unitOfWork.ShoppingCarts.GetDetails
                (s => s.ApplicationUserId == userId && s.ProductId == shoppingCart.ProductId);

            if(shoppingCartFromDb != null)
            {
                shoppingCartFromDb.Count += shoppingCart.Count;
                await this._unitOfWork.ShoppingCarts.Update(shoppingCartFromDb);
                await this._unitOfWork.Save();
            }
            else
            {
                await this._unitOfWork.ShoppingCarts.Add(shoppingCart);
                await this._unitOfWork.Save();

                var shoppingCartsCount = await this._unitOfWork.ShoppingCarts.GetAll
                    (s => s.ApplicationUserId == userId);
                int count = 0;
                foreach (var item in shoppingCartsCount)
                {
                    count++;
                }
                this.HttpContext.Session.SetInt32(StaticDetails.SessionCart, count);
            }

            await this._unitOfWork.Save();

            return this.RedirectToAction($"{nameof(this.Index)}", "Home");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using BooksStore.Models.ViewModels;
using BooksStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BooksStore.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	[Route("[area]/[controller]")]
	public class CartController : Controller
	{
		private readonly ILogger<CartController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
		{
			this._logger = logger;
			this._unitOfWork = unitOfWork;
		}

		private decimal? GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			if (shoppingCart.Count <= 50)
			{
				return shoppingCart?.Product?.Price;
			}
			else
			{
				if (shoppingCart.Count <= 100)
				{
					return shoppingCart?.Product?.Price50;
				}
				return shoppingCart?.Product?.Price100;
			}
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> Index()
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
				nameof(CartController), nameof(this.Index));

			var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var shoppingCartsList = await this._unitOfWork.ShoppingCarts.GetAll
				(s => s.ApplicationUserId == userId, includeProperties: "Product");

			var shoppingCartVM = new ShoppingCartVM()
			{
				OrderHeader = new()
			};

			shoppingCartVM.OrderHeader.OrderTotal = 0;

			foreach (var shoppingCart in shoppingCartsList)
			{
				var price = this.GetPriceBasedOnQuantity(shoppingCart);
				shoppingCart.Price = price;
				shoppingCartVM.OrderHeader.OrderTotal += price * shoppingCart.Count;
			}

			shoppingCartVM.ShoppingCarts = shoppingCartsList;

            if (this.HttpContext.Session.GetInt32(StaticDetails.SessionCart) == null ||
                this.HttpContext.Session.GetInt32(StaticDetails.SessionCart) == 0)
            {
                ViewBag.DisableSummary = true;
            }

            return View(shoppingCartVM);
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> Summary()
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
				nameof(CartController), nameof(this.Summary));

			var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var shoppingCartsList = await this._unitOfWork.ShoppingCarts.GetAll
				(s => s.ApplicationUserId == userId, includeProperties: "Product");

			var shoppingCartVM = new ShoppingCartVM()
			{
				OrderHeader = new()
			};

			var applicationUser = await this._unitOfWork.ApplicationUsers
				.GetDetails(u => u.Id == userId, includeProperties: "Company");

			if (applicationUser != null)
			{
				shoppingCartVM.OrderHeader.ApplicationUser = applicationUser;
				shoppingCartVM.OrderHeader.Name = applicationUser.Name;
				shoppingCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
				shoppingCartVM.OrderHeader.StreetAddress = applicationUser.StreetAddress;
				shoppingCartVM.OrderHeader.City = applicationUser.City;
				shoppingCartVM.OrderHeader.State = applicationUser.State;
				shoppingCartVM.OrderHeader.PostalCode = applicationUser.PostalCode;

				shoppingCartVM.OrderHeader.OrderTotal = 0;
			}


			foreach (var shoppingCart in shoppingCartsList)
			{
				var price = this.GetPriceBasedOnQuantity(shoppingCart);
				shoppingCart.Price = price;
				shoppingCartVM.OrderHeader.OrderTotal += price * shoppingCart.Count;
			}

			shoppingCartVM.ShoppingCarts = shoppingCartsList;

			ViewBag.PaymentList = new List<SelectListItem>()
			{
                new SelectListItem()
				{
                    Text = StaticDetails.PaymentCash.ToString(),
                    Value = StaticDetails.PaymentCash
                },
                new SelectListItem()
                {
                    Text = StaticDetails.PaymentCreditCard.ToString(),
                    Value = StaticDetails.PaymentCreditCard,
					Disabled = true
                },
            };

			return View(shoppingCartVM);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<IActionResult> SummaryPOST([FromForm] ShoppingCartVM shoppingCartVM)
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action post method",
				nameof(CartController), nameof(this.SummaryPOST));

			var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			shoppingCartVM.ShoppingCarts = await this._unitOfWork.ShoppingCarts.GetAll
				(s => s.ApplicationUserId == userId, includeProperties: "Product");

			if (shoppingCartVM.OrderHeader != null)
			{
				shoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
				shoppingCartVM.OrderHeader.ApplicationUserId = userId;

				var applicationUser = await this._unitOfWork.ApplicationUsers.GetDetails
					(u => u.Id == userId, includeProperties: "Company");

				shoppingCartVM.OrderHeader.OrderTotal = 0;

				foreach (var cart in shoppingCartVM.ShoppingCarts)
				{
					cart.Price = this.GetPriceBasedOnQuantity(cart);
					shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
				}

				if (applicationUser?.CompanyId.GetValueOrDefault() == 0)
				{
					//it is a regular customer account and we need to capture payment
					shoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
					shoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
				}
				else
				{
					//it is a company user
					shoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
					shoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusApproved;
				}

				shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
				shoppingCartVM.OrderHeader.ShippingDate = DateTime.Now.AddDays(7);
				shoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
				shoppingCartVM.OrderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));

				await this._unitOfWork.OrderHeaders.Add(shoppingCartVM.OrderHeader);
				await this._unitOfWork.Save();

				foreach (var cart in shoppingCartVM.ShoppingCarts)
				{
					OrderDetail orderDetail = new()
					{
						ProductId = cart.ProductId,
						OrderHeaderId = shoppingCartVM.OrderHeader.Id,
						Price = (decimal)cart.Price,
						Count = cart.Count
					};

					await this._unitOfWork.OrderDetails.Add(orderDetail);
					await this._unitOfWork.Save();
				}
			}

			return this.RedirectToAction("OrderConfirmation", "Cart", new
			{
				id = shoppingCartVM?.OrderHeader?.Id
			});
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> OrderConfirmation([FromQuery] int? id)
		{
			if(id == null)
			{
				return this.NotFound();
			}

			var orderHeader = await this._unitOfWork.OrderHeaders.GetDetails
				(o => o.Id == id, includeProperties: "ApplicationUser");

			if(orderHeader != null)
			{
				var shoppingCarts = await this._unitOfWork.ShoppingCarts.GetAll
					(s => s.ApplicationUserId == orderHeader.ApplicationUserId);
				await this._unitOfWork.ShoppingCarts.RemoveRange(shoppingCarts);
				await this._unitOfWork.Save();

				this.HttpContext.Session.Clear();
			}

			return View(id);
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> Plus([FromQuery] int? cartId)
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
				nameof(CartController), nameof(this.Plus));

			if (cartId == null)
			{
				return this.NotFound();
			}
			var shoppingCart = await this._unitOfWork.ShoppingCarts.GetDetails
				(s => s.Id == cartId);
			if (shoppingCart == null)
			{
				return this.NotFound();
			}

			shoppingCart.Count += 1;
			await this._unitOfWork.ShoppingCarts.Update(shoppingCart);
			await this._unitOfWork.Save();

			return this.RedirectToAction("Index", "Cart");
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> Minus([FromQuery] int? cartId)
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
				nameof(CartController), nameof(this.Minus));

			if (cartId == null)
			{
				return this.NotFound();
			}
			var shoppingCart = await this._unitOfWork.ShoppingCarts.GetDetails
				(s => s.Id == cartId, tracked: true);
			if (shoppingCart == null)
			{
				return this.NotFound();
			}

			if (shoppingCart.Count <= 1)
			{
                var shoppingCartsList = await this._unitOfWork.ShoppingCarts.GetAll
					(s => s.ApplicationUserId == shoppingCart.ApplicationUserId);
                int count = 0;
                foreach (var item in shoppingCartsList)
                {
                    count++;
                }

                this.HttpContext.Session.SetInt32(StaticDetails.SessionCart, count - 1);

                await this._unitOfWork.ShoppingCarts.Remove(shoppingCart);
				await this._unitOfWork.Save();
			}
			else
			{
				shoppingCart.Count -= 1;
				await this._unitOfWork.ShoppingCarts.Update(shoppingCart);
				await this._unitOfWork.Save();
			}

			return this.RedirectToAction("Index", "Cart");
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> Remove([FromQuery] int? cartId)
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
				nameof(CartController), nameof(this.Remove));

			if (cartId == null)
			{
				return this.NotFound();
			}
			var shoppingCart = await this._unitOfWork.ShoppingCarts.GetDetails
				(s => s.Id == cartId, tracked: true);
			if (shoppingCart == null)
			{
				return this.NotFound();
			}

			var shoppingCartsList = await this._unitOfWork.ShoppingCarts.GetAll
				(s => s.ApplicationUserId == shoppingCart.ApplicationUserId);
			int count = 0;
            foreach (var item in shoppingCartsList)
            {
				count++;
            }

            this.HttpContext.Session.SetInt32(StaticDetails.SessionCart, count - 1);

            await this._unitOfWork.ShoppingCarts.Remove(shoppingCart);
			await this._unitOfWork.Save();

			return this.RedirectToAction("Index", "Cart");
		}
	}
}

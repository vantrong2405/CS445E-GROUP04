using ArtAuctionManage.DataAccess.Repositories.IRepositories;
using ArtAuctionManage.Models;
using ArtAuctionManage.Models.ViewModels;
using ArtAuctionManage.Utilities;
using ArtAuctionManage.Web.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

namespace ArtAuctionManage.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
    //[Authorize(Roles = $"{StaticDetails.Role_Admin},{StaticDetails.Role_Employee}")]
    [Authorize]
	[Route("[area]/[controller]")]
	public class OrderController : Controller
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		public OrderController(ILogger<OrderController> logger, IUnitOfWork unitOfWork)
		{
			this._logger = logger;
			this._unitOfWork = unitOfWork;
		}

		[HttpGet]
		[Route("[action]")]
		public IActionResult Index()
		{
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(OrderController), nameof(this.Index));

            return View();
		}

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> OrderDetails([FromQuery]int? orderId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(OrderController), nameof(this.OrderDetails));

            if (!orderId.HasValue)
            {
                return this.NotFound();
            }
            var orderHeader = await this._unitOfWork.OrderHeaders.GetDetails
                (oh => oh.Id == orderId, includeProperties: "ApplicationUser");
            if(orderHeader == null)
            {
                return this.NotFound();
            }
            var listOfOrderDetails = await this._unitOfWork.OrderDetails.GetAll
                (od => od.OrderHeaderId == orderHeader.Id);

            var orderVM = new OrderVM()
            {
                OrderHeader = orderHeader,
                OrderDetails = listOfOrderDetails
            };

            return View(orderVM);
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Employee + "," + StaticDetails.Role_Admin)]
        [Route("[action]")]
        public async Task<IActionResult> UpdateOrderDetails([FromForm]OrderVM orderVM)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action POST method",
                nameof(OrderController), nameof(this.UpdateOrderDetails));

            if (orderVM == null || orderVM.OrderHeader == null)
            {
                return this.NotFound();
            }
            var orderHeaderFromDb = await this._unitOfWork.OrderHeaders.GetDetails
                (o => o.Id == orderVM.OrderHeader.Id);
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.Name = orderVM.OrderHeader.Name;
                orderHeaderFromDb.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
                orderHeaderFromDb.StreetAddress = orderVM.OrderHeader.StreetAddress;
                orderHeaderFromDb.City = orderVM.OrderHeader.City;
                orderHeaderFromDb.State = orderVM.OrderHeader.State;
                orderHeaderFromDb.PostalCode = orderVM.OrderHeader.PostalCode;
                if (!string.IsNullOrEmpty(orderVM.OrderHeader.Carrier))
                {
                    orderHeaderFromDb.Carrier = orderVM.OrderHeader.Carrier;
                }
                if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
                {
                    orderHeaderFromDb.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
                }

                await this._unitOfWork.OrderHeaders.Update(orderHeaderFromDb);
                await this._unitOfWork.Save();
            }

            TempData["Success"] = "Order Details Updated Successfully.";

            return this.RedirectToAction(nameof(OrderDetails), new { orderId = orderHeaderFromDb?.Id });
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Employee + "," + StaticDetails.Role_Admin)]
        [Route("[action]")]
        public async Task<IActionResult> StartProcessing([FromForm] OrderVM orderVM)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action POST method",
                nameof(OrderController), nameof(this.StartProcessing));

            if (orderVM == null || orderVM.OrderHeader == null)
            {
                return this.NotFound();
            }
            await this._unitOfWork.OrderHeaders.UpdateStatus
                (orderVM.OrderHeader.Id, StaticDetails.StatusInProcess);
            await this._unitOfWork.Save();

            TempData["Success"] = "Order Processing Successfully.";

            return this.RedirectToAction(nameof(OrderDetails), new { orderId = orderVM.OrderHeader?.Id });
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Employee + "," + StaticDetails.Role_Admin)]
        [Route("[action]")]
        public async Task<IActionResult> ShipOrder([FromForm] OrderVM orderVM)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action POST method",
               nameof(OrderController), nameof(this.ShipOrder));

            if (orderVM == null || orderVM.OrderHeader == null)
            {
                return this.NotFound();
            }

            var orderHeader = await _unitOfWork.OrderHeaders.GetDetails
                (u => u.Id == orderVM.OrderHeader.Id);
            if (orderHeader != null)
            {               
                orderHeader.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
                orderHeader.Carrier = orderVM.OrderHeader.Carrier;
                orderHeader.OrderStatus = StaticDetails.StatusShipped;
                orderHeader.ShippingDate = DateTime.Now;
                if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
                {
                    orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
                }

                await this._unitOfWork.OrderHeaders.Update(orderHeader);
                await this._unitOfWork.Save();
                TempData["Success"] = "Order Shipped Successfully.";
            }
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderVM.OrderHeader?.Id });
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Employee + "," + StaticDetails.Role_Admin)]
        [Route("[action]")]
        public async Task<IActionResult> CancelOrder([FromForm] OrderVM orderVM)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action POST method",
               nameof(OrderController), nameof(this.ShipOrder));

            if (orderVM == null || orderVM.OrderHeader == null)
            {
                return this.NotFound();
            }

            var orderHeader = await _unitOfWork.OrderHeaders.GetDetails
                (u => u.Id == orderVM.OrderHeader.Id);
            if (orderHeader != null)
            {
                await this._unitOfWork.OrderHeaders.UpdateStatus
                    (orderHeader.Id, StaticDetails.StatusCancelled, StaticDetails.PaymentStatusRejected);

                //orderVM.OrderHeader.OrderStatus = StaticDetails.StatusCancelled;
                //orderVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusRejected;

                //await this._unitOfWork.OrderHeaders.Update(orderHeader);
                await this._unitOfWork.Save();
                TempData["Success"] = "Order Canceled Successfully.";
            }
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderVM.OrderHeader?.Id });
        }

        #region API Calls
        [HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> GetAllOrders([FromQuery]string? status)
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action API get method",
				nameof(OrderController), nameof(this.GetAllOrders));

            IEnumerable<OrderHeader> orderHeaders;

            if (this.User.IsInRole(StaticDetails.Role_Admin) || this.User.IsInRole(StaticDetails.Role_Employee))
            {
			    orderHeaders = await this._unitOfWork.OrderHeaders.GetAll
				    (includeProperties: "ApplicationUser");
            } else
            {
                var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                orderHeaders = await this._unitOfWork.OrderHeaders.GetAll
                    (o => o.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            }

			switch(status)
			{
                case "pending":
                    orderHeaders = orderHeaders.Where(o => o.PaymentStatus == StaticDetails.PaymentStatusPending);
                    break;
                case "delayed":
                    orderHeaders = orderHeaders.Where(o => o.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(o => o.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(o => o.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(o => o.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;
            }

            return this.Json(new
            {
                Data = orderHeaders
            });
        }
		#endregion
	}
}

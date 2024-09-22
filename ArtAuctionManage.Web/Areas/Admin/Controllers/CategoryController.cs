using ArtAuctionManage.DataAccess.Database;
using ArtAuctionManage.DataAccess.Repositories.IRepositories;
using ArtAuctionManage.Models.DTOs;
using ArtAuctionManage.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtAuctionManage.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    [Route("[area]/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController
            (ICategoryRepository categoryRepo, IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Index));
            var categories = await _unitOfWork.Categories.GetAll();

            var categoriesResponse = categories.Select(c => c.ToCategoryResponse()).ToList();

            return View(categoriesResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Create));

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create
            ([FromForm] CategoryAddRequest categoryAddRequest)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action post method",
                nameof(CategoryController), nameof(this.Create));

            if (categoryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(categoryAddRequest));
            }

            if (categoryAddRequest.Name == null)
            {
                throw new ArgumentException(nameof(categoryAddRequest));
            }

            if (categoryAddRequest.Name.ToLower() == categoryAddRequest.DisplayOrder.ToString())
            {
                ModelState.AddModelError
                    (nameof(categoryAddRequest.Name), "Category name and display order can not be same");
            }

            if (ModelState.IsValid)
            {
                var category = categoryAddRequest.ToCategory();

                await _unitOfWork.Categories.Add(category);
                await _unitOfWork.Save();

                TempData["Success"] = "Create category succesfully";

                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromQuery] int? categoryId)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Edit));

            if (categoryId == null)
            {
                return NotFound();
            }

            var category = await _categoryRepo.GetDetails(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }

            var categoryUpdateRequest = category.ToCategoryUpdateRequest();

            return View(categoryUpdateRequest);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateRequest categoryUpdateRequest)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} post action method",
                nameof(CategoryController), nameof(this.Edit));

            if (!ModelState.IsValid)
            {
                return View(categoryUpdateRequest);
            }

            var category = categoryUpdateRequest.ToCategory();
            await _categoryRepo.Update(category);

            TempData["Success"] = "Update category successfully";

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromQuery] int? categoryId)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Delete));

            if (categoryId == null)
            {
                return NotFound();
            };

            var category = await _categoryRepo.GetDetails(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }

            var categoryResponse = category.ToCategoryResponse();

            return View(categoryResponse);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeletePOST([FromForm] int? Id)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} delete action method",
                nameof(CategoryController), nameof(this.DeletePOST));

            if (Id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepo.GetDetails(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepo.Remove(category);

            TempData["Success"] = "Delete category successfully";

            return RedirectToAction("Index", "Category");
        }
    }
}

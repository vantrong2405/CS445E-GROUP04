using ArtAuctionManage.DataAccess.Repositories.IRepositories;
using ArtAuctionManage.Models.DTOs;
using ArtAuctionManage.Models.ViewModels;
using ArtAuctionManage.Models;
using ArtAuctionManage.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtAuctionManage.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    [Route("[area]/[controller]")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompanyController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, ILogger<CompanyController> logger, IWebHostEnvironment webHostEnvironment)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._webHostEnvironment = webHostEnvironment;
        }

        private CompanyResponse ConvertCompanyToCompanyResponse(Company company)
        {
            var companyResponse = company.ToCompanyResponse();

            return companyResponse;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CompanyController), nameof(this.Index));
            var companies = await _unitOfWork.Companies.GetAll();

            var companiesResponse = companies.Select
                (p => this.ConvertCompanyToCompanyResponse(p)).ToList();

            return View(companiesResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CompanyController), nameof(this.Create));

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create
            ([FromForm] CompanyUpdateRequest companyUpdateRequest)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action post method",
                nameof(CompanyController), nameof(this.Create));

            if (companyUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(companyUpdateRequest));
            }

            if (companyUpdateRequest.Name == null)
            {
                throw new ArgumentException(nameof(companyUpdateRequest));
            }

            var company = companyUpdateRequest.ToCompany();

            await this._unitOfWork.Companies.Add(company);
            await this._unitOfWork.Save();

            TempData["Success"] = "Create company succesfully";

            return RedirectToAction("Index", "Company");

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromQuery] int? companyId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CompanyController), nameof(this.Edit));

            if (companyId == null)
            {
                return NotFound();
            }

            var company = await this._unitOfWork.Companies.GetDetails(c => c.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var companyUpdateRequest = company.ToCompanyUpdateRequest();
            
            return View(companyUpdateRequest);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromForm] CompanyUpdateRequest companyUpdateRequest)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} post action method",
                nameof(CompanyController), nameof(this.Edit));
           

            var company = companyUpdateRequest.ToCompany();

            await this._unitOfWork.Companies.Update(company);
            await this._unitOfWork.Save();

            TempData["Success"] = "Update company successfully";

            return RedirectToAction("Index", "Company");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromQuery] int? companyId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CompanyController), nameof(this.Delete));

            if (companyId == null)
            {
                return NotFound();
            };

            var company = await this._unitOfWork.Companies.GetDetails
                (p => p.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var companyResponse = this.ConvertCompanyToCompanyResponse(company);

            return View(companyResponse);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeletePOST([FromForm] int? Id)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} delete action method",
                nameof(CompanyController), nameof(this.DeletePOST));

            if (Id == null)
            {
                return NotFound();
            }

            var company = await this._unitOfWork.Companies.GetDetails(p => p.Id == Id);
            if (company == null)
            {
                return NotFound();
            }

            await this._unitOfWork.Companies.Remove(company);
            await this._unitOfWork.Save();

            TempData["Success"] = "Delete company successfully";

            return RedirectToAction("Index", "Company");
        }      
    }
}

using ArtAuctionManage.DataAccess.Database;
using ArtAuctionManage.Models;
using ArtAuctionManage.Models.ViewModels;
using ArtAuctionManage.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArtAuctionManage.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = StaticDetails.Role_Admin)]
    //[Authorize]
    [Route("[area]/[controller]")]
    public class UserController : Controller
    {
        private readonly ArtAuctionManageDbContext _db;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ArtAuctionManageDbContext db, ILogger<UserController> logger, UserManager<IdentityUser> userManager)
        {
            this._db = db;
            this._logger = logger;
            this._userManager = userManager;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(UserController), nameof(this.Index));

            return this.View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> RoleManagement([FromQuery] string? userId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(UserController), nameof(this.RoleManagement));

            if (userId == null)
            {
                return this.NotFound();
            }

            var role = await this._db.UserRoles.FirstOrDefaultAsync
                (u => u.UserId == userId);
            var roleId = role?.RoleId;

            var roleVM = new RoleManagementVM()
            {
                ApplicationUser = await this._db.ApplicationUsers.Include(u => u.Company).FirstOrDefaultAsync
                    (u => u.Id == userId),
                RoleList = this._db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = this._db.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (roleVM.ApplicationUser != null)
            {
                roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault
                    (u => u.Id == roleId)?.Name;
            }

            return this.View(roleVM);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RoleManagement
            ([FromForm] RoleManagementVM roleManagementVM)
        {
            var roleID = this._db.UserRoles.FirstOrDefault
                (u => u.UserId == roleManagementVM.ApplicationUser.Id)?.RoleId;
            var oldRole = _db.Roles.FirstOrDefault(u => u.Id == roleID)?.Name;

            if (!(roleManagementVM.ApplicationUser?.Role == oldRole))
            {
                //a role was updated
                var applicationUser = await this._db.ApplicationUsers.FirstOrDefaultAsync
                    (u => u.Id == roleManagementVM.ApplicationUser.Id);
                if (roleManagementVM.ApplicationUser.Role == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if (oldRole == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }
            return RedirectToAction("Index");
        }

        #region API calls
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get API method",
                nameof(UserController), nameof(this.GetAllUsers));

            var users = await this._db.ApplicationUsers
                .Include(u => u.Company).ToListAsync();

            var usersRoles = await this._db.UserRoles.ToListAsync();
            var roles = await this._db.Roles.ToListAsync();

            foreach (var user in users)
            {
                var roleId = usersRoles.FirstOrDefault(u => u.UserId == user.Id)?.RoleId;
                if (roleId == null)
                {
                    user.Role = "Didn't assign role yet";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(r => r.Id == roleId)?.Name;
                }

                if (user.Company == null)
                {
                    user.Company = new Company();
                    user.Company.Name = string.Empty;
                }
            }

            return this.Json(new
            {
                Data = users
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LockUnlock([FromQuery] string? id)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action POST API method",
                nameof(UserController), nameof(this.LockUnlock));

            var userFromDb = await this._db.ApplicationUsers.FirstOrDefaultAsync
                (u => u.Id == id);
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and we need to unlock them
                userFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            await this._db.SaveChangesAsync();

            return this.Json(new { success = true, message = "Lock/Unlock User Successful" });
        }
        #endregion
    }
}

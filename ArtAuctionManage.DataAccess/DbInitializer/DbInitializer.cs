using BooksStore.DataAccess.Database;
using BooksStore.Models;
using BooksStore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BooksStoreDbContext _db;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, BooksStoreDbContext db)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._db = db;
        }

        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (this._db.Database.GetPendingMigrations().Count() > 0)
                {
                    this._db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            //create roles if they are not created
            if (!this._roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
            {
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();

            }

            //if roles are not created, then we will create admin user as well
            //this._userManager.CreateAsync(new ApplicationUser
            //{
            //    UserName = "admin4",
            //    Email = "admin4@gmail.com",
            //    Name = "Nguyễn Thanh Admin4",
            //    PhoneNumber = "0906413506",
            //    StreetAddress = "54 Hải Hồ",
            //    State = "Hải Châu",
            //    PostalCode = "23422",
            //    City = "Đà Nẵng",
            //}, password: "@Nguyenthanhanh123").GetAwaiter().GetResult();

            //var user = this._db.ApplicationUsers.FirstOrDefaultAsync
            //    (u => u.Email == "admin4@gmail.com").GetAwaiter().GetResult();

            //if (user != null)
            //{
            //    var result = _userManager.ChangePasswordAsync
            //        (user, "@Nguyenthanhanh123", "@Nguyenthanhanh123").GetAwaiter().GetResult();

            //    if (result.Succeeded)
            //    {
            //        Console.WriteLine("Password changed successfully.");
            //    }
            //    else
            //    {
            //        foreach (var error in result.Errors)
            //        {
            //            Console.WriteLine($"Error: {error.Description}");
            //        }
            //    }
            //    this._userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
            //}

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@dotnetmastery.com",
                Email = "admin@dotnetmastery.com",
                Name = "Bhrugen Patel",
                PhoneNumber = "1112223333",
                StreetAddress = "test 123 Ave",
                State = "IL",
                PostalCode = "23422",
                City = "Chicago"
            }, "Admin123*").GetAwaiter().GetResult();


            ApplicationUser? user = _db.ApplicationUsers.FirstOrDefault
                (u => u.Email == "admin@dotnetmastery.com");

            _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();

            return;
        }
    }
}

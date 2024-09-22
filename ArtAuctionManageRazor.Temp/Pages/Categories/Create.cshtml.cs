using ArtAuctionManageRazor.Temp.Database;
using ArtAuctionManageRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtAuctionManageRazor.Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ArtAuctionManageRazorDbContext _db;
        public Category Category { get; set; }
        public CreateModel(ArtAuctionManageRazorDbContext db)
        {
            this._db = db;
            this.Category = new Category();
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            this._db.Categories.Add(this.Category);
            await this._db.SaveChangesAsync();

            return this.RedirectToAction("Index", "Categories");
        }
    }
}

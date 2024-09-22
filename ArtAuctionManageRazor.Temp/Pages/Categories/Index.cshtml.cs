using ArtAuctionManageRazor.Temp.Database;
using ArtAuctionManageRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtAuctionManageRazor.Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ArtAuctionManageRazorDbContext _db;
        public List<Category> Categories { get; set; }
        public IndexModel(ArtAuctionManageRazorDbContext db)
        {
            this._db = db;
            this.Categories = new List<Category>();
        }

        public void OnGet()
        {
            this.Categories = this._db.Categories.ToList();
        }
    }
}

using BooksStoreRazor.Temp.Database;
using BooksStoreRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksStoreRazor.Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly BooksStoreRazorDbContext _db;
        public List<Category> Categories { get; set; }
        public IndexModel(BooksStoreRazorDbContext db)
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

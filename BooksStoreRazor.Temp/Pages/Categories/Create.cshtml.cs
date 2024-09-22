using BooksStoreRazor.Temp.Database;
using BooksStoreRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksStoreRazor.Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly BooksStoreRazorDbContext _db;
        public Category Category { get; set; }
        public CreateModel(BooksStoreRazorDbContext db)
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

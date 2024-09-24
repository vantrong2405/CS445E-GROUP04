using System.ComponentModel.DataAnnotations;
using BooksStore.Models;

namespace BooksStore.Models.DTOs
{
    public class CategoryUpdateRequest
    {
        [Required(ErrorMessage = "Category Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Category name only accepts letters")]
        [MaxLength(40, ErrorMessage = "Category name must be less than 40 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Display Order is required")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }

        public Category ToCategory()
        {
            return new Category()
            {
                Id = Id,
                Name = Name,
                DisplayOrder = DisplayOrder
            };
        }
    }
}

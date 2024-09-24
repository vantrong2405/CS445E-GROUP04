using System.ComponentModel.DataAnnotations;
using BooksStore.Models;

namespace BooksStore.Models.DTOs
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int DisplayOrder { get; set; }
    }
    public static class CategoryExtensions
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
            };
        }

        public static CategoryUpdateRequest ToCategoryUpdateRequest(this Category category)
        {
            return new CategoryUpdateRequest()
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
            };
        }
    }
}

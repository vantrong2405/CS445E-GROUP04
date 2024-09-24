using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.DTOs
{
    public class ProductUpdateRequest
    {
        [Required(ErrorMessage = "Product Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(40, ErrorMessage = "Title cannot exceed 40 characters.")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [MaxLength(100, ErrorMessage = "ISBN cannot exceed 100 characters.")]
        [Display(Name = "International Standard Book Number")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(40, ErrorMessage = "Author cannot exceed 40 characters.")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "List Price is required.")]
        [Display(Name = "List Price")]
        [Range(1, 1000, ErrorMessage = "List Price must be between 1 and 1000.")]
        public decimal ListPrice { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Price for 50+ is required.")]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000, ErrorMessage = "Price for 50+ must be between 1 and 1000.")]
        public decimal Price50 { get; set; }

        [Required(ErrorMessage = "Price for 100+ is required.")]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000, ErrorMessage = "Price for 100+ must be between 1 and 1000.")]
        public decimal Price100 { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                ISBN = this.ISBN,
                Author = this.Author,
                ListPrice = this.ListPrice,
                Price = this.Price,
                Price50 = this.Price50,
                Price100 = this.Price100,
                CategoryId = this.CategoryId,
                ImageUrl = this.ImageUrl,
            };
        }
    }
}

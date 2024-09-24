using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BooksStore.Models.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        [Display(Name = "International Standard Book Number")]
        public string? ISBN { get; set; }

        public string? Author { get; set; }

        [Display(Name = "List Price")]
        public decimal ListPrice { get; set; }

        [Display(Name = "Price for 1-50")]
        public decimal Price { get; set; }

        [Display(Name = "Price for 50+")]
        public decimal Price50 { get; set; }

        [Display(Name = "Price for 100+")]
        public decimal Price100 { get; set; }

        [Display(Name = "Category")]
        public string? CategoryName { get; set; }

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
    }
    public static class ProductExtensions
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ISBN = product.ISBN,
                Author = product.Author,
                ListPrice = product.ListPrice,
                Price = product.Price,
                Price50 = product.Price50,
                Price100 = product.Price100,
                ImageUrl = product.ImageUrl,
            };
        }
        public static ProductUpdateRequest ToProductUpdateRequest(this Product product)
        {
            return new ProductUpdateRequest()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ISBN = product.ISBN,
                Author = product.Author,
                ListPrice = product.ListPrice,
                Price = product.Price,
                Price50 = product.Price50,
                Price100 = product.Price100,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
            };
        }
    }
}

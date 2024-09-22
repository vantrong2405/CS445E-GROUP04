using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "International Standard Book Number")]
        public string? ISBN { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public decimal ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public decimal Price { get; set; }


        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public decimal Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public decimal Price100 { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(this.CategoryId))]
        public Category? Category { get; set; }
    }
}

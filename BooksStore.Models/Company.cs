using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models
{
    public class Company
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? StreetAddress { get; set; }

        [MaxLength(40)]
        public string? City { get; set; }

        [MaxLength(40)]
        public string? State { get; set; }

        [Required]
        [StringLength(5)]
        public string? PostalCode { get; set; }

        [Required]
        [StringLength(10)]
        public string? PhoneNumber { get; set; }
    }
}

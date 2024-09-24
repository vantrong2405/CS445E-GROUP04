using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.DTOs
{
    public class CompanyAddRequest
    {
        [Required(ErrorMessage = "Company name is required.")]
        [MaxLength(40, ErrorMessage = "Company name cannot exceed 40 characters.")]
        public string? Name { get; set; }

        [MaxLength(100, ErrorMessage = "Street address cannot exceed 100 characters.")]
        public string? StreetAddress { get; set; }

        [MaxLength(40, ErrorMessage = "City name cannot exceed 40 characters.")]
        public string? City { get; set; }

        [MaxLength(40, ErrorMessage = "State name cannot exceed 40 characters.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [MaxLength(5, ErrorMessage = "Postal code cannot exceed 5 characters.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number must be 10 characters.")]
        public string? PhoneNumber { get; set; }

        public Company ToCompany()
        {
            return new Company()
            {
                Name = Name,
                StreetAddress = StreetAddress,
                City = City,
                State = State,
                PostalCode = PostalCode,
                PhoneNumber = PhoneNumber
            };
        }
    }
}

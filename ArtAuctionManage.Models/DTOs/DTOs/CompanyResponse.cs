using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.DTOs
{
    public class CompanyResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [DisplayName("Street Address")]
        public string? StreetAddress { get; set; }

        [MaxLength(40)]
        public string? City { get; set; }

        [MaxLength(40)]
        public string? State { get; set; }

        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
    }
    public static class CompanyExtensions
    {
        public static CompanyResponse ToCompanyResponse(this Company company)
        {
            return new CompanyResponse()
            {
                Id = company.Id,
                Name = company.Name,
                StreetAddress = company.StreetAddress,
                City = company.City,
                State = company.State,
                PostalCode = company.PostalCode,
                PhoneNumber = company.PhoneNumber
            };
        }
        public static CompanyUpdateRequest ToCompanyUpdateRequest(this Company company)
        {
            return new CompanyUpdateRequest()
            {
                Id = company.Id,
                Name = company.Name,
                StreetAddress = company.StreetAddress,
                City = company.City,
                State = company.State,
                PostalCode = company.PostalCode,
                PhoneNumber = company.PhoneNumber
            };
        }
    }
}

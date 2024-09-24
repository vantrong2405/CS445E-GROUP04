using BooksStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.ViewModels
{
    public class ProductUpdateVM
    {
        public required ProductUpdateRequest ProductUpdateRequest { get; set; }

        [ValidateNever]
        public IFormFile? File { get; set; }
    }
}

using BooksStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.ViewModels
{
    public class ProductCreateVM
    {
        public required ProductAddRequest ProductAddRequest { get; set; }

        [ValidateNever]
        public IFormFile? File { get; set; }
    }
}

using BooksStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.ViewModels
{
    public class CompanyCreateVM
    {
        public required CompanyAddRequest CompanyAddRequest { get; set; }
    }
}

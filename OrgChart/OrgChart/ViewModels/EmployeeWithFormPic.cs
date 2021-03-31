using Microsoft.AspNetCore.Http;
using OrgChart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.ViewModels
{
    public class EmployeeWithFormPic
    {
        public Employee Employee { get; set; }

        [Display(Name = "Change Picture")]
        public IFormFile FormPicture { get; set; }
    }
}

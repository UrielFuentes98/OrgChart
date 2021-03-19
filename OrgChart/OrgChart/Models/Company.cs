using OrgChart.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public class Company
    {
        public string OwnerName { get; set; }
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [Display(Name = "Company Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a address")]
        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }

        public List<Employee> Employees { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        [Display(Name = "First Name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        [Display(Name = "Last Name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a position")]
        [Display(Name = "Position")]
        [StringLength(30)]
        public string Position { get; set; }

        [Required(ErrorMessage = "Please enter a email")]
        [Display(Name = "Email")]
        [StringLength(30)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [Display(Name = "Phone")]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter a office location")]
        [Display(Name = "Office")]
        [StringLength(50)]
        public string Office { get; set; }

        public int ManagerId { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}

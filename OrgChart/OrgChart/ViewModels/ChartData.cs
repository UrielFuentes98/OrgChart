using OrgChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.ViewModels
{
    public class ChartData
    {
        public ChartData(List<EmployeeWithImgString> employees, EmployeeWithImgString manager)
        {
            Employees = employees;
            Manager = manager;
        }
        public List<EmployeeWithImgString> Employees { get; set; }

        public EmployeeWithImgString Manager { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrgChart.Models;
using OrgChart.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            //_logger = logger;
            this.employeeRepository = employeeRepository;
        }

        public IActionResult Chart(int id)
        {
            IEnumerable<Employee> employeesGroup;
            Employee manager;

            if (id == 0) 
            {
                employeesGroup = employeeRepository.GetSubordinates(1);
                manager = employeeRepository.GetEmployeeInfo(1);
            } 
            else
            {
                employeesGroup = employeeRepository.GetSubordinates(id);
                manager = employeeRepository.GetEmployeeInfo(id);
            }

            var employeesView = new ChartList(employeesGroup, manager); 

            return View(employeesView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

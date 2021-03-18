using Microsoft.AspNetCore.Mvc;
using OrgChart.Models;
using OrgChart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Controllers
{
    public class DiagramController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public DiagramController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public IActionResult List()
        {
            var allEmployees = employeeRepository.GetAllEmployees();

            return View(allEmployees);
        }

        public IActionResult Chart(int empId)
        {
            IEnumerable<Employee> employeesGroup;
            Employee manager;

            //If no id provided default to first employee added
            if (empId == 0)
            {
                employeesGroup = employeeRepository.GetSubordinates(1);
                manager = employeeRepository.GetEmployeeInfo(1);
            }
            else
            {
                employeesGroup = employeeRepository.GetSubordinates(empId);
                manager = employeeRepository.GetEmployeeInfo(empId);
            }

            var employeesView = new ChartList(employeesGroup, manager);

            return View(employeesView);
        }
    }
}

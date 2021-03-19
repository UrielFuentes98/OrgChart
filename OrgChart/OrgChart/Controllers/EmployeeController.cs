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
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            this.employeeRepository = employeeRepository;
        }

        //TO DO - Check how to delete functions
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

        public IActionResult Detail(int empId)
        {
            var employee = employeeRepository.GetEmployeeInfo(empId);

            return View(employee);
        }

        public IActionResult Edit(int empId, bool isEdit)
        {
            
            //If edit mode, bind employe of id passed else create
            //employee with boss with passed id
            if (isEdit)
            {
                var employee = employeeRepository.GetEmployeeInfo(empId);
                return View(employee);
            }
            else
            {
                var employee = new Employee() { ManagerId = empId };
                return View(employee);
            }
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //Check if employee with givel id is in DB. If so update, else add.
            var actualEmployee = employeeRepository.GetEmployeeInfo(employee.EmployeeId);
            if (actualEmployee != null)
            {
                employeeRepository.UpdateEmployee(employee);
                return RedirectToAction("Chart", "Diagram", employee.EmployeeId);
            }
            else
            {
                employeeRepository.AddEmployee(employee);
                return RedirectToAction("Chart", "Diagram", employee.ManagerId);
            }
            
        }

        public IActionResult DeletePreview(int empId)
        {
            //If employee has subordinates cant be deleted because
            //it would broke the org employee tree.

            var empHasSub = employeeRepository.HasSubordiantes(empId);

            if (empHasSub)
            {
                ViewBag.Message = "Sorry employee has subordinates so cannot be deleted.";
                return View("_ErrorMessage");

            }
            else
            {
                var empToDelete = employeeRepository.GetEmployeeInfo(empId);
                return View("Delete/DeletePreview", empToDelete);
            }

        }

        public IActionResult DeleteConfirmation(int empId)
        {
            var empToDelete = employeeRepository.GetEmployeeInfo(empId);
            employeeRepository.DeleteEmployee(empToDelete);
            return View("Delete/DeleteConfirmation", empToDelete.ManagerId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

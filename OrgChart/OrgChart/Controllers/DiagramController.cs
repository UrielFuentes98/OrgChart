using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrgChart.Models;
using OrgChart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Controllers
{
    [Authorize]
    public class DiagramController : Controller
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICompanyRepository companyRepository;

        public DiagramController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository)
        {
            this.logger = logger;
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
        }
        public IActionResult List(List<EmployeeWithImgString> employees)
        {
            var companyId = HttpContext.Session.GetInt32("company_id");

            //Check if a company was selected
            if (companyId > 0)
            {
                ViewBag.CompanyName = companyRepository.GetCompanyById(companyId).Name;

                //If employee list passed, render it, if not render all employees registered.
                if (!employees.Any())
                {
                    var allEmployees = employeeRepository.GetAllEmployees(companyId);
                    var employeesWithImgString = new List<EmployeeWithImgString>();

                    //Obtain image encoded string for each employee
                    foreach (var employee in allEmployees)
                    {
                        employeesWithImgString.Add(EmployeeWithImgString.GetEmployeeWithImgString(employee)); 
                    }

                    return View(employeesWithImgString);
                }
                else
                {
                    return View(employees);
                }
            }
            else
            {
                ViewBag.CompanyName = "Select a company";
                //No company, so create empty Employee list to render
                var noEmployees = new List<EmployeeWithImgString>();
                return View(noEmployees);
            }

        }

        public IActionResult Chart(int empId)
        {
            var companyId = HttpContext.Session.GetInt32("company_id");
            IEnumerable<Employee> employeesGroup;
            Employee manager;
            var empWithImgString = new List<EmployeeWithImgString>();
            var manWithImgString = new EmployeeWithImgString();

            //Check if a company is selected.
            if (companyId > 0)
            {
                ViewBag.CompanyName = companyRepository.GetCompanyById(companyId).Name;

                //If no id provided default to first employee added to company
                if (empId == 0)
                {
                    manager = employeeRepository.GetFirstEmployeeInfo(companyId);

                    //If manager found look for its subordinates
                    if (manager != null)
                    {
                        manWithImgString = EmployeeWithImgString.GetEmployeeWithImgString(manager);
                        employeesGroup = employeeRepository.GetSubordinates(manager.EmployeeId, companyId);

                        //Loop through each employee to convert byte array to image string encoded if image exists
                        foreach (var employee in employeesGroup)
                        {
                            empWithImgString.Add(EmployeeWithImgString.GetEmployeeWithImgString(employee));

                        }
                    }
                    logger.LogInformation("Rendering chart from top");
                }
                else
                {
                    employeesGroup = employeeRepository.GetSubordinates(empId, companyId);

                    //Loop through each employee to convert byte array to image string encoded if image exists
                    foreach (var employee in employeesGroup)
                    {
                        empWithImgString.Add(EmployeeWithImgString.GetEmployeeWithImgString(employee));

                    }

                    manager = employeeRepository.GetEmployeeInfo(empId, companyId);
                    manWithImgString = EmployeeWithImgString.GetEmployeeWithImgString(manager);
                }
            }
            else
            {
                ViewBag.CompanyName = "Select a company";
            }
            var employeesView = new ChartData(empWithImgString, manWithImgString);

            return View(employeesView);
        }

        public IActionResult SearchEmployee(string empName)
        {
            var companyId = HttpContext.Session.GetInt32("company_id");
            var employeesFound = employeeRepository.GetEmployeesByName(empName, companyId);

            if (employeesFound.Any())
            {
                //If more than one employee show List if only one show detail
                if (employeesFound.Count() > 1)
                {
                    var employeesWithImgString = new List<EmployeeWithImgString>();

                    //Obtain image encoded string for each employee
                    foreach (var employee in employeesFound)
                    {
                        employeesWithImgString.Add(EmployeeWithImgString.GetEmployeeWithImgString(employee));
                    }

                    return View("List", employeesWithImgString);
                }
                else
                {
                    return RedirectToAction("Detail", "Employee", new { empId = employeesFound.First().EmployeeId });
                }

            }
            else
            {
                ViewBag.Message = "Sorry. No Employee was Found.";
                return View("_ErrorMessage");
            }
        }  
    }
}

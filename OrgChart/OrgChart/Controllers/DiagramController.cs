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
        public IActionResult List(IEnumerable<Employee> employees)
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
                    return View(allEmployees);
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
                var noEmployees = new List<Employee>();
                return View(noEmployees);
            }

        }

        public IActionResult Chart(int empId)
        {
            var user = User.Identity.Name;
            if (user != null)
            {
                var companyId = HttpContext.Session.GetInt32("company_id");
                IEnumerable<Employee> employeesGroup = null;
                Employee manager = null;

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
                            employeesGroup = employeeRepository.GetSubordinates(manager.EmployeeId, companyId);
                        }
                    }
                    else
                    {
                        employeesGroup = employeeRepository.GetSubordinates(empId, companyId);
                        manager = employeeRepository.GetEmployeeInfo(empId, companyId);
                    }
                }
                else
                {
                    ViewBag.CompanyName = "Select a company";
                }
                var employeesView = new ChartData(employeesGroup, manager);

                return View(employeesView);
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }
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
                    return View("List", employeesFound);
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

﻿using Microsoft.AspNetCore.Http;
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

        public DiagramController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            this.logger = logger;
            this.employeeRepository = employeeRepository;
        }
        public IActionResult List(IEnumerable<Employee> employees)
        {
            if (!employees.Any())
            {
                var allEmployees = employeeRepository.GetAllEmployees();
                return View(allEmployees);
            }
            else
            {
                return View(employees);
            }

        }

        public IActionResult Chart(int empId)
        {
            var user = User.Identity.Name;
            if (user != null)
            {
                var companyId = HttpContext.Session.GetInt32("company_id");
                logger.LogInformation(companyId.ToString());
                IEnumerable<Employee> employeesGroup;
                Employee manager;

                //If no id provided default to first employee added
                if (empId == 0)
                {
                    manager = employeeRepository.GetFirstEmployeeInfo(companyId);
                    if (manager != null)
                    {
                        employeesGroup = employeeRepository.GetSubordinates(manager.EmployeeId);
                    }
                    else
                    {
                        employeesGroup = null;
                    }
                }
                else
                {
                    employeesGroup = employeeRepository.GetSubordinates(empId);
                    manager = employeeRepository.GetEmployeeInfo(empId);
                }

                var employeesView = new ChartList(employeesGroup, manager);

                return View(employeesView);
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }
        }

        public IActionResult SearchEmployee(string empName)
        {
            var employeesFound = employeeRepository.GetEmployeesByName(empName);

            if (employeesFound.Any())
            {
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

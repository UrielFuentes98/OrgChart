using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrgChart.Models;
using OrgChart.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IPictureRepository pictureRepository;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IPictureRepository pictureRepository)
        {
            _logger = logger;
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
            this.pictureRepository = pictureRepository;
        }

        public IActionResult Detail(int empId)
        {
            var companyId = HttpContext.Session.GetInt32("company_id");
            var employee = employeeRepository.GetEmployeeInfo(empId, companyId);
            var empWithImgString = EmployeeWithImgString.GetEmployeeWithImgString(employee);
            return View(empWithImgString);
        }

        public IActionResult Edit(int empId, bool isEdit)
        {
            var companyId = HttpContext.Session.GetInt32("company_id");
            //If edit mode, bind employe of id passed else create
            //employee with manager with passed id
            if (isEdit)
            {
                var employee = employeeRepository.GetEmployeeInfo(empId, companyId);
                var empWithNoFile = new EmployeeWithFormPic() { Employee = employee };
                return View(empWithNoFile);
            }
            else
            {
                var newEmployee = new Employee() { ManagerId = empId };
                var employeeWithPicture = new EmployeeWithFormPic()
                { Employee = newEmployee };

                return View(employeeWithPicture);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeWithFormPic employeeWithPicture)
        {
            //If invalid form rerender to inform user.
            if (!ModelState.IsValid)
            {
                return View();
            }

            //If employee Id valid, update employee if not, create new one.
            if (employeeWithPicture.Employee.EmployeeId > 0)
            {
                //Save image data to db if image was uploaded
                if (employeeWithPicture.FormPicture != null)
                {
                    using var memoryStream = new MemoryStream();
                    await employeeWithPicture.FormPicture.CopyToAsync(memoryStream);

                    employeeWithPicture.Employee.EmployeePicture = new EmployeePicture()
                    {
                        Content = memoryStream.ToArray(),
                        Name = employeeWithPicture.FormPicture.FileName,
                    };
                    employeeRepository.UpdateEmployee(employeeWithPicture.Employee);
                }
                else
                {
                    //Get previous employee picture if picture was not changed.
                    var employeePicture = employeeRepository
                                            .GetEmployeeInfo(employeeWithPicture.Employee.EmployeeId, employeeWithPicture.Employee.CompanyId)
                                            .EmployeePicture;

                    employeeWithPicture.Employee.EmployeePicture = employeePicture;
                    employeeRepository.UpdateEmployee(employeeWithPicture.Employee);
                }



                return RedirectToAction("Chart", "Diagram", employeeWithPicture.Employee.EmployeeId);
            }
            else
            {
                var companyId = HttpContext.Session.GetInt32("company_id");
                employeeWithPicture.Employee.CompanyId = companyId;

                //Save image data to db if image was uploaded
                if (employeeWithPicture.FormPicture != null)
                {
                    using var memoryStream = new MemoryStream();
                    await employeeWithPicture.FormPicture.CopyToAsync(memoryStream);

                    employeeWithPicture.Employee.EmployeePicture = new EmployeePicture()
                    {
                        Content = memoryStream.ToArray(),
                        Name = employeeWithPicture.FormPicture.FileName
                    };
                }
                else
                {
                    employeeWithPicture.Employee.EmployeePicture = new EmployeePicture();
                }

                employeeRepository.AddEmployee(employeeWithPicture.Employee);
                return RedirectToAction("Chart", "Diagram", employeeWithPicture.Employee.ManagerId);

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
                var companyId = HttpContext.Session.GetInt32("company_id");
                var empToDelete = employeeRepository.GetEmployeeInfo(empId, companyId);

                //Add image encoded string to object for card rendering.
                var empWithImgString = EmployeeWithImgString.GetEmployeeWithImgString(empToDelete);
                return View("Delete/DeletePreview", empWithImgString);
            }

        }

        public IActionResult DeleteConfirmation(int empId)
        {
            var companyId = HttpContext.Session.GetInt32("company_id");
            var empToDelete = employeeRepository.GetEmployeeInfo(empId, companyId);
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

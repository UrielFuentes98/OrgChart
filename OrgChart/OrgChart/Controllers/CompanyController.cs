using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrgChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public IActionResult List()
        {
            var UserCompanies = companyRepository.GetCompaniesOfUser(User.Identity.Name);
            return View(UserCompanies);
        }

        public IActionResult Edit(int compId, bool isEdit)
        {

            //If edit mode, bind company of id passed else create new company

            if (isEdit)
            {
                var company = companyRepository.GetCompanyById(compId);
                return View(company);
            }
            else
            {
                var company = new Company();
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            //If model is not valid return to inform the user
            if (!ModelState.IsValid)
            {
                return View();
            }

            //Check if company id is valid to determine if create or update.
            if (company.CompanyId > 0)
            {
                companyRepository.UpdateCompany(company);
            }
            else
            {
                var userName = User.Identity.Name;
                company.OwnerName = userName;
                companyRepository.CreateComapany(company);
            }

            return RedirectToAction("List");
        }

        public IActionResult SelectCompany (int companyId)
        {

            //Add selected company id to the session for tracking
            HttpContext.Session.SetInt32("company_id", companyId);
            return RedirectToAction("Chart", "Diagram");
        }

        public IActionResult DeletePreview(int compId)
        {
            //If company has employees cant be deleted to avoid 
            //letting employees without a company in DB.

            var compHasSub = companyRepository.HasEmployees(compId);

            if (compHasSub)
            {
                ViewBag.Message = "Sorry company has employees so cannot be deleted.";
                return View("_ErrorMessage");

            }
            else
            {
                var compToDelete = companyRepository.GetCompanyById(compId);
                return View("Delete/DeletePreview", compToDelete);
            }

        }

        public IActionResult DeleteConfirmation(int compId)
        {
            var compToDelete = companyRepository.GetCompanyById(compId);
            companyRepository.DeleteCompany(compToDelete);
            return View("Delete/DeleteConfirmation");
        }
    }
}

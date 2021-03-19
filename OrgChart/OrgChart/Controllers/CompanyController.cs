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

            //If edit mode, bind employe of id passed else create
            //employee with boss with passed id
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
    }
}

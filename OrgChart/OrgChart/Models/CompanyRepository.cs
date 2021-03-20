using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly OrgChartDbContext db;

        public CompanyRepository(OrgChartDbContext db)
        {
            this.db = db;
        }
        public void CreateComapany(Company company)
        {
            db.Companies.Add(company);
            db.SaveChanges();
        }

        public void CreateEmployee(Employee newEmployee, int? companyId)
        {
            var selectedCompany = db.Companies.SingleOrDefault(c => c.CompanyId == companyId);

            if (selectedCompany != null)
            {
                selectedCompany.Employees.Add(newEmployee);
                db.SaveChanges();
            }
        }

        public void DeleteCompany(Company company)
        {
            db.Companies.Remove(company);
            db.SaveChanges();
        }

        public IEnumerable<Company> GetCompaniesOfUser(string userName)
        {
            return db.Companies.Where(c => c.OwnerName == userName);
        }

        public Company GetCompanyById(int? compId)
        {
            return db.Companies.SingleOrDefault(c => c.CompanyId == compId);
        }

        public bool HasEmployees(int compId)
        {
            var hasEmployees = db.Employees.Any(e => e.CompanyId == compId);

            return hasEmployees;
        }

        public void UpdateCompany(Company company)
        {
            db.Companies.Update(company);
            db.SaveChanges();
        }
    }
}

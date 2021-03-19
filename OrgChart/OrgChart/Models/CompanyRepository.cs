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

        public IEnumerable<Company> GetCompanies()
        {
            return db.Companies;
        }

        public Company GetCompanyById(int compId)
        {
            return db.Companies.SingleOrDefault(c => c.CompanyId == compId);
        }

        public void UpdateCompany(Company company)
        {
            db.Companies.Update(company);
            db.SaveChanges();
        }
    }
}

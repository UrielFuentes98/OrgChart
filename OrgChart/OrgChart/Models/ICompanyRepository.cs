using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public interface ICompanyRepository 
    {
        IEnumerable<Company> GetCompaniesOfUser(string userName);
        void CreateComapany(Company company);

        void UpdateCompany(Company company);
        Company GetCompanyById(int? compId);

        void CreateEmployee(Employee newEmployee, int? companyId);

        void DeleteCompany(Company company);
        bool HasEmployees(int compId);
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OrgChartDbContext db;

        public EmployeeRepository(OrgChartDbContext db)
        {
            this.db = db;
        }

        public void AddEmployee(Employee employee)
        {
            db.Employees.Add(employee);
            db.SaveChanges();
        }

        public void DeleteEmployee(Employee employee)
        {
            db.Employees.Remove(employee);
            db.SaveChanges();
        }

        public IEnumerable<Employee> GetAllEmployees(int? compId = 0)
        {
            return db.Employees.Where(e => e.CompanyId == compId);
        }

        //Search if input string is in the first name or last name. 
        public IEnumerable<Employee> GetEmployeesByName(string inputName, int? companyId = 0)
        {

            //Check for input in first name
            var empByFirstName = db.Employees.AsNoTracking().Where(e => e.FirstName
                .StartsWith(inputName) && e.CompanyId == companyId);
            if (!empByFirstName.Any())
            {
                //Check for input in last name
                var empByLastName = db.Employees.AsNoTracking().Where(e => e.LastName
                    .StartsWith(inputName) && e.CompanyId == companyId);

                if (!empByLastName.Any())
                {
                    //Check for input in both first and last name
                    var inputNoSpaces = Regex.Replace(inputName, @"\s+", "");
                    var empByFullName = db.Employees.ToList().Where(e => Regex.Replace(e.FirstName + e.LastName, @"\s+", "")
                        .StartsWith(inputNoSpaces) && e.CompanyId == companyId);
                    return empByFullName;
                }
                return empByLastName;
            }
            return empByFirstName;
        }

        public Employee GetEmployeeInfo(int employeeId, int? companyId = 0)
        {
            return db.Employees.AsNoTracking().SingleOrDefault(e => e.EmployeeId == employeeId && e.CompanyId == companyId);
        }

        public IEnumerable<Employee> GetSubordinates(int managerId, int? companyId = 0)
        {
            return db.Employees.Where(e => e.ManagerId == managerId && e.CompanyId == companyId).OrderBy(e => e.LastName);
        }

        public bool HasSubordiantes(int employeeId)
        {
            var subordinate = db.Employees.FirstOrDefault(e => e.ManagerId == employeeId);

            if (subordinate != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            db.Employees.Update(employee);
            db.SaveChanges();
        }

        public Employee GetFirstEmployeeInfo(int? companyId)
        {
            return db.Employees.FirstOrDefault(e => e.CompanyId == companyId);
        }
    }
}

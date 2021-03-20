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

        public IEnumerable<Employee> GetAllEmployees()
        {
            return db.Employees;
        }

        //Search if input string is in the first name or last name. 
        public IEnumerable<Employee> GetEmployeesByName(string inputName)
        {
            
            //Check for input in first name
            var empByFirstName = db.Employees.AsNoTracking().Where(e => e.FirstName.StartsWith(inputName));
            if (!empByFirstName.Any())
            {
                //Check for input in last name
                var empByLastName = db.Employees.AsNoTracking().Where(e => e.LastName.StartsWith(inputName));

                if (!empByLastName.Any())
                {
                    //Check for input in both first and last name
                    var inputNoSpaces = Regex.Replace(inputName, @"\s+", "");
                    var empByFullName = db.Employees.ToList().Where(e => Regex.Replace(e.FirstName + e.LastName, @"\s+", "").StartsWith( inputNoSpaces));
                    return empByFullName;
                }
                return empByLastName;
            }
            var x = empByFirstName.First();
            return empByFirstName;
        }

        public Employee GetEmployeeInfo(int employeeId)
        {
            return db.Employees.AsNoTracking().SingleOrDefault(e => e.EmployeeId == employeeId);
        }

        public IEnumerable<Employee> GetSubordinates(int managerId)
        {
            return db.Employees.Where(e => e.ManagerId == managerId).OrderBy(e => e.LastName);
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

        public Employee GetFirstEmployeeInfo( int? companyId)
        {
            var x = db.Employees;
            return db.Employees.FirstOrDefault(e => e.CompanyId == companyId);
        }
    }
}

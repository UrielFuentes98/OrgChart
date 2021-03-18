using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Employee GetEmployeeInfo(int employeeId)
        {
            return db.Employees.AsNoTracking().SingleOrDefault(e => e.EmployeeId == employeeId);
        }

        public IEnumerable<Employee> GetSubordinates(int managerId)
        {
            return db.Employees.Where(e => e.ManagerId == managerId).OrderBy(e => e.LastName);
        }

        public void UpdateEmployee(Employee employee)
        {
            db.Employees.Update(employee);
            db.SaveChanges();
        }
    }
}

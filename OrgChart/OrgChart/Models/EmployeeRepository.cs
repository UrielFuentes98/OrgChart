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
        public Employee GetEmployeeInfo(int employeeId)
        {
            return db.Employees.SingleOrDefault(e => e.EmployeeId == employeeId);
        }

        public IEnumerable<Employee> GetSubordinates(int managerId)
        {
            return db.Employees.Where(e => e.ManagerId == managerId).OrderBy(e => e.LastName);
        }
    }
}

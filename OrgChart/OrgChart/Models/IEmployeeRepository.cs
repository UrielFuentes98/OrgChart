using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetSubordinates(int managerId);

        Employee GetEmployeeInfo(int employeeId);

        void AddEmployee(Employee employee);

        void UpdateEmployee(Employee employee);

        void DeleteEmployee(Employee employee);
    }
}

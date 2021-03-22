using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(int? compId = 0);
        IEnumerable<Employee> GetSubordinates(int managerId, int? companyId = 0);

        Employee GetEmployeeInfo(int employeeId, int? companyId = 0);

        Employee GetFirstEmployeeInfo(int? companyId);
        void AddEmployee(Employee employee);

        void UpdateEmployee(Employee employee);

        void DeleteEmployee(Employee employee);

        bool HasSubordiantes(int employeeId);

        IEnumerable<Employee> GetEmployeesByName(string inputName, int? companyId = 0);
    }
}

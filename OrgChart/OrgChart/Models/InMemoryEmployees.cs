using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public class InMemoryEmployees : IEmployeeRepository
    {
        private List<Employee> employees;
        public InMemoryEmployees()
        {
            employees = new List<Employee>()
            {
                new Employee{EmployeeId = 1, FirstName = "Uriel", LastName = "Fuentes", Email = "mail@test.com", Phone = "1234-567-890", Office="4th Building, 403", ManagerId = 0},
                new Employee{EmployeeId = 2, FirstName = "Juan", LastName = "Perez", Email = "mail@test.com", Phone = "1234-567-890", Office="4th Building, 403", ManagerId = 1},
                new Employee{EmployeeId = 3, FirstName = "Rosa", LastName = "Martinez", Email = "mail@test.com", Phone = "1234-567-890", Office="4th Building, 403", ManagerId = 1},
                new Employee{EmployeeId = 4, FirstName = "Pedro", LastName = "Gomez", Email = "mail@test.com", Phone = "1234-567-890", Office="4th Building, 403", ManagerId = 2},
                new Employee{EmployeeId = 5, FirstName = "Jose", LastName = "Gonzalez", Email = "mail@test.com", Phone = "1234-567-890", Office="4th Building, 403", ManagerId = 2}
            };
        }

        public void AddEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployeeInfo(int employeeId)
        {
            return employees.SingleOrDefault(e => e.EmployeeId == employeeId);
        }

        public IEnumerable<Employee> GetSubordinates(int managerId)
        {
            return employees.Where(e => e.ManagerId == managerId).OrderBy(e => e.LastName);
        }

        public void UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}

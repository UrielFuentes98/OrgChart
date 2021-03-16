using OrgChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.ViewModels
{
    public class ChartList
    {
        public ChartList(IEnumerable<Employee> employees, Employee manager)
        {
            Employees = employees;
            Manager = manager;
        }
        public IEnumerable<Employee> Employees { get; set; }

        public Employee Manager { get; set; }
    }
}

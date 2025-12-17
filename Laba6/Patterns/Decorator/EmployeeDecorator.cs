using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public abstract class EmployeeDecorator : Employee
    {
        public Employee _employee;

        public EmployeeDecorator(Employee employee)
            : base(employee.Name, employee.BaseSalary, employee.BankService)
        {
            _employee = employee;
        }

        public override abstract string GetInfo();

        public override double CalculateSalary()
        {
            return _employee.CalculateSalary();
        }
    }
}

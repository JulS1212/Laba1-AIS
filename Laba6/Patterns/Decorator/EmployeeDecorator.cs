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

        public EmployeeDecorator(Employee employee): base(employee?.Name ?? "", employee?.BaseSalary ?? 0,employee?.BankService)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public override abstract string GetInfo();

        public override double CalculateSalary()
        {
            return _employee.CalculateSalary();
        }
        public override double GetCreditApprovalProbability()
        {
            return _employee.GetCreditApprovalProbability();
        }
    }
}

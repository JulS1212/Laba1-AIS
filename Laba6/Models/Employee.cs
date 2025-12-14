using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public abstract class Employee
    {
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        public IBankService BankService { get; set; }

        public Employee(string name, double baseSalary, IBankService bankService)
        {
            Name = name;
            BaseSalary = baseSalary;
            BankService = bankService;
        }
        public abstract string GetInfo();
        public virtual double CalculateSalary()
        {
            return BankService.CalculateSalary(BaseSalary);
        }
    }
}

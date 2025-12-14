using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class Engineer : Employee
    {
        public Engineer(string name, double baseSalary, IBankService bankService)
            : base(name, baseSalary, bankService)
        {
        }

        public override string GetInfo()
        {
            return $"Инженер: {Name}";
        }
    }
}

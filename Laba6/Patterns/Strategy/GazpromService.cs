using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class GazpromService : IBankService
    {
        public double CalculateSalary(double baseSalary)
        {
            return baseSalary * 0.985;
        }

        public string GetServiceName()
        {
            return "Газпромбанк (комиссия 1.5%)";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class SberbankService : IBankService
    {
        public double CalculateSalary(double baseSalary)
        {
            return baseSalary * 0.99;
        }

        public string GetServiceName()
        {
            return "Сбербанк (комиссия 1%)";
        }
    }
}

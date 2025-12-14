using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public interface IBankService
    {
        double CalculateSalary(double baseSalary);
        string GetServiceName();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class Scientist : Employee
    {
        public Scientist(string name, double baseSalary, IBankService bankService)
            : base(name, baseSalary, bankService)
        {
        }

        public override string GetInfo()
        {
            return $"Учёный: {Name} (Кредит:  {GetCreditApprovalProbability() * 100:F1} %)";
        }

        public override double GetCreditApprovalProbability()
        {
            // Учёные: база 60% + 0.1% за каждые 1000 руб зарплаты
            double baseProbability = 0.6; // 60%
            double salaryBonus = (BaseSalary / 1000) * 0.001; // 0.1% за каждую 1000 руб
            return Math.Min(0.95, baseProbability + salaryBonus); // Макс 95%
        }
    }
}

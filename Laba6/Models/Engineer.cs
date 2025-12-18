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
            return $"Инженер: {Name} (Кредит: {GetCreditApprovalProbability() * 100:F1}%)";
        }

        public override double GetCreditApprovalProbability()
        {
            // Инженеры: база 50% + 0.15% за каждые 1000 руб зарплаты
            double baseProbability = 0.5; // 50%
            double salaryBonus = (BaseSalary / 1000) * 0.0015; // 0.15% за каждую 1000 руб
            return Math.Min(0.9, baseProbability + salaryBonus); // Макс 90%
        }
    }
}

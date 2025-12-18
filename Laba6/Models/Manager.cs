using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class Manager : Employee
    {
        public Manager(string name, double baseSalary, IBankService bankService)
            : base(name, baseSalary, bankService)
        {
        }

        public override string GetInfo()
        {
            return $"Менеджер: {Name} (Кредит: {GetCreditApprovalProbability() * 100:F1}%)";
        }

        public override double GetCreditApprovalProbability()
        {
            // Менеджеры: база 70% + 0.08% за каждые 1000 руб зарплаты
            double baseProbability = 0.7; // 70%
            double salaryBonus = (BaseSalary / 1000) * 0.0008; // 0.08% за каждую 1000 руб
            return Math.Min(0.98, baseProbability + salaryBonus); // Макс 98%
        }
    }
}

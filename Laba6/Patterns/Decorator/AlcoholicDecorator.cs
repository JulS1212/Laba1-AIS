using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6.Patterns.Decorator
{
    public class AlcoholicDecorator : EmployeeDecorator
    {
        private const double REDUCTION_PERCENT = 0.3; // -30%

        public AlcoholicDecorator(Employee employee)
            : base(employee) { }

        public override string GetInfo()
        {
            double originalProb = _employee.GetCreditApprovalProbability() * 100;
            double currentProb = GetCreditApprovalProbability() * 100;
            return $"{_employee.GetInfo()}, Алкоголик (кредит: {currentProb:F1}%)";
        }

        // ПЕРЕОПРЕДЕЛЯЕМ - уменьшаем вероятность на 30%
        public override double GetCreditApprovalProbability()
        {
            double originalProbability = _employee.GetCreditApprovalProbability();
            double reducedProbability = originalProbability * (1 - REDUCTION_PERCENT);

            // Гарантируем, что вероятность не упадёт ниже 10%
            return Math.Max(0.1, reducedProbability);
        }
    }
}

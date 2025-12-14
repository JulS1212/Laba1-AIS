using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class IntramediateEnglishSerializer : EmployeeDecorator
    {
        public string ExaminationTitle { get; set; }
        public int YearOfSertificate { get; set; }

        public IntramediateEnglishSerializer(Employee employee, string examinationTitle, int yearOfSertificate)
            : base(employee)
        {
            ExaminationTitle = examinationTitle;
            YearOfSertificate = yearOfSertificate;
        }

        public override string GetInfo()
        {
            return $"{_employee.GetInfo()}, Английский: Intermediate (сертификат '{ExaminationTitle}', {YearOfSertificate} г.)";
        }
    }
}

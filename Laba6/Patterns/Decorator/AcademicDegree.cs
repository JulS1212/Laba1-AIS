using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba6
{
    public class AcademicDegree : EmployeeDecorator
    {
        public string DissertationTitle { get; set; }  
        public int Year { get; set; }
        public string ScienceArea { get; set; }

        public AcademicDegree(Employee employee, string dissertationTitle, int year, string scienceArea): base(employee)
        {
            DissertationTitle = dissertationTitle;
            Year = year;
            ScienceArea = scienceArea;
        }

        public override string GetInfo()
        {
            return $"{_employee.GetInfo()}, Учёная степень: {ScienceArea} ('{DissertationTitle}', {Year} г.)";
        }
    }
}

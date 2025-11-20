using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogical.Interfaces
{
    public interface IPaintingValidator
    {
        bool Validate(string title, string artist, int year, string genre);
        string ValidateWithMessage(string title, string artist, int year, string genre);
    }
}

using BusinessLogical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogical.Validators
{
    public class PaintingValidator : IPaintingValidator
    {
        //public bool Validate(string title, string artist, int year, string genre)
        //{
        //    return !string.IsNullOrWhiteSpace(title) &&
        //           !string.IsNullOrWhiteSpace(artist) &&
        //           year >= 1000 && year <= DateTime.Now.Year &&
        //           !string.IsNullOrWhiteSpace(genre);
        //}

        public string ValidateWithMessage(string title, string artist, int year, string genre)
        {
            if (string.IsNullOrWhiteSpace(title)) return "Название не может быть пустым";
            if (string.IsNullOrWhiteSpace(artist)) return "Автор не может быть пустым";
            if (year < 1000 || year > DateTime.Now.Year) return "Некорректный год";
            if (string.IsNullOrWhiteSpace(genre)) return "Жанр не может быть пустым";
            return null; // всё ок
        }
    }
}

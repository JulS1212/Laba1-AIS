using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogical;
namespace ConsoleApp7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logic logic = new Logic();
            Console.WriteLine("Введите название картины:");
            string title = Console.ReadLine();
            Console.WriteLine("Введите автора картины:");
            string artist = Console.ReadLine();
            Console.WriteLine("Введите год картины:");
            int year = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите жанр картины:");
            string genre = Console.ReadLine();

            logic.AddPainting(title, artist, year, genre);
        }
    }
}

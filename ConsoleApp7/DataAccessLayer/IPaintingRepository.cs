using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IPaintingRepository : IRepository<Painting>  // наследуем от базового
    {
        // ДОБАВЛЯЕМ методы, которые есть в Logic:
        IEnumerable<Painting> GetByYearRange(int startYear, int endYear);
        //bool PaintingExists(string title, string artist);
        Painting GetPainting(string title, string artist);
    }
}

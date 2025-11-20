using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogical.Interfaces
{
    public interface IPaintingService
    {
        void AddPainting(string title, string artist, int year, string genre);
        bool PaintingExists(string title, string artist);
        Painting GetPainting(string title, string artist);
        bool DeletePainting(string title, string artist);
        List<Painting> GetAllPaintings();
        bool UpdatePainting(string oldTitle, string oldArtist, string newTitle, string newArtist, int newYear, string newGenre);
        Dictionary<string, List<Painting>> GroupByGenre();
        List<Painting> GetPaintingsByYearRange(int startYear, int endYear);
        List<string> GetAll();
        List<Painting> SortByTitleAscending();
        List<Painting> SortByTitleDescending();
    }
}

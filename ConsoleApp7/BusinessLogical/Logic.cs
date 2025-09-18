using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace BusinessLogical
{
    public class Logic
    {
        List<Painting> Paintings = new List<Painting>();
        public void AddPainting(string title, string artist, int year, string genre)
        {
            Painting painting = new Painting()
            {
                Title = title,
                Artist = artist,
                Year = year,
                Genre = genre
            };
            Paintings.Add(painting);
        }

        public bool PaintingExists(string title, string artist)
        {
            return Paintings.Any(p =>
                p.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                p.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        }
        public Painting GetPainting(string title, string artist)
        {
            return Paintings.FirstOrDefault(p =>
                p.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && p.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        }
        public bool DeletePainting(string title, string artist)
        {
            var painting = GetPainting(title, artist);
            if (painting != null)
            {
                Paintings.Remove(painting);
                return true;
            }
            return false;
        }
        
        public List<Painting> GetAllPaintings()
        {
            return new List<Painting>(Paintings);
        }
        public bool UpdatePainting(string oldTitle, string oldArtist, string newTitle, string newArtist, int newYear, string newGenre)
        {
            // Ищем по названию И автору!
            var painting = Paintings.FirstOrDefault(p =>
                p.Title.Equals(oldTitle, StringComparison.OrdinalIgnoreCase) &&
                p.Artist.Equals(oldArtist, StringComparison.OrdinalIgnoreCase));

            if (painting != null)
            {
                // Проверяем уникальность нового названия+автора
                if ((!oldTitle.Equals(newTitle, StringComparison.OrdinalIgnoreCase) ||
                     !oldArtist.Equals(newArtist, StringComparison.OrdinalIgnoreCase)) &&
                    Paintings.Any(p =>
                        p.Title.Equals(newTitle, StringComparison.OrdinalIgnoreCase) &&
                        p.Artist.Equals(newArtist, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("Картина с таким названием и автором уже существует!");
                }

                painting.Title = newTitle;
                painting.Artist = newArtist;
                painting.Year = newYear;
                painting.Genre = newGenre;

                return true;
            }
            return false;
        }
        public Dictionary<string, List<Painting>> GroupByGenre()
        {
            if (Paintings == null || Paintings.Count == 0)
            {
                return new Dictionary<string, List<Painting>>();
            }

            return Paintings
                .GroupBy(p => p.Genre)  // Группируем по жанру
                .OrderBy(g => g.Key)     // Сортируем по названию жанра
                .ToDictionary(g => g.Key, g => g.OrderBy(p => p.Title).ToList()); // Сортируем картины внутри жанра
        }
        // 5. Бизнес-функция 1: Группировка картин по жанрам
        public List<Painting> GetPaintingsByYearRange(int startYear, int endYear)
        {
            return Paintings
                .Where(p => p.Year >= startYear && p.Year <= endYear)
                .OrderBy(p => p.Year)
                .ToList();
        }

        public List<string> GetAll()
        {
            List<string> result = new List<string>();
            foreach (Painting painting in Paintings)
            {
                result.Add($"{painting.Title} - {painting.Artist} ({painting.Year}), {painting.Genre}");
            }
            return result;
        }
    }
}

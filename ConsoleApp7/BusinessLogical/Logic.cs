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
        /// <summary>
        /// Добавляет новую картину в коллекцию
        /// </summary>
        /// <param name="title">Название картины</param>
        /// <param name="artist">Автор картины</param>
        /// <param name="year">Год создания картины</param>
        /// <param name="genre">Жанр картины</param>
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

        /// <summary>
        /// Проверяет существование картины в коллекции по названию и автору
        /// </summary>
        /// <param name="title">Название картины для поиска</param>
        /// <param name="artist">Автор картины для поиска</param>
        /// <returns>True если картина существует, иначе False</returns>
        public bool PaintingExists(string title, string artist)
        {
            return Paintings.Any(p =>
                p.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                p.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Получает картину из коллекции по названию и автору
        /// </summary>
        /// <param name="title">Название картины для поиска</param>
        /// <param name="artist">Автор картины для поиска</param>
        /// <returns>Объект Painting если найден, иначе null</returns>
        public Painting GetPainting(string title, string artist)
        {
            return Paintings.FirstOrDefault(p =>
                p.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && p.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Удаляет картину из коллекции по названию и автору
        /// </summary>
        /// <param name="title">Название картины для удаления</param>
        /// <param name="artist">Автор картины для удаления</param>
        /// <returns>True если картина была удалена, иначе False</returns>
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

        /// <summary>
        /// Получает все картины из коллекции
        /// </summary>
        /// <returns>Список всех объектов Painting</returns>
        public List<Painting> GetAllPaintings()
        {
            return new List<Painting>(Paintings);
        }

        /// <summary>
        /// Обновляет информацию о существующей картине
        /// </summary>
        /// <param name="oldTitle">Текущее название картины</param>
        /// <param name="oldArtist">Текущий автор картины</param>
        /// <param name="newTitle">Новое название картины</param>
        /// <param name="newArtist">Новый автор картины</param>
        /// <param name="newYear">Новый год создания картины</param>
        /// <param name="newGenre">Новый жанр картины</param>
        /// <returns>True если картина была обновлена, иначе False</returns>
        /// <exception cref="ArgumentException">Выбрасывается если новая комбинация названия и автора уже существует</exception>
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

        /// <summary>
        /// Группирует картины по жанрам с сортировкой
        /// </summary>
        /// <returns>Словарь где ключ - жанр, значение - отсортированный список картин этого жанра</returns>
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

        /// <summary>
        /// Получает картины созданные в указанном диапазоне лет
        /// </summary>
        /// <param name="startYear">Начальный год диапазона</param>
        /// <param name="endYear">Конечный год диапазона</param>
        /// <returns>Отсортированный список картин созданных в указанном диапазоне лет</returns>
        public List<Painting> GetPaintingsByYearRange(int startYear, int endYear)
        {
            return Paintings
                .Where(p => p.Year >= startYear && p.Year <= endYear)
                .OrderBy(p => p.Year)
                .ToList();
        }

        /// <summary>
        /// Получает текстовое представление всех картин в коллекции
        /// </summary>
        /// <returns>Список строк в формате "Название - Автор (Год), Жанр"</returns>
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

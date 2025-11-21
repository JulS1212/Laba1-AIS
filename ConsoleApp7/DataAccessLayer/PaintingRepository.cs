using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PaintingRepository : IPaintingRepository
    {
        private readonly IRepository<Painting> _repository; // используем готовый DapperRepository

        public void Add(Painting item) => _repository.Add(item);
        public void Delete(int id) => _repository.Delete(id);
        public void Update(Painting item) => _repository.Update(item);
        public IEnumerable<Painting> ReadAll() => _repository.ReadAll();
        public Painting ReadById(int id) => _repository.ReadById(id);
        public PaintingRepository(IRepository<Painting> repository)
        {
            _repository = repository;
        }

        //public bool PaintingExists(string title, string artist)
        //{
        //    return _repository.ReadAll().Any(p =>
        //        p.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
        //        p.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        //}

        public Painting GetPainting(string title, string artist)
        {
            return _repository.ReadAll().FirstOrDefault(p =>
                p.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                p.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Painting> GetByYearRange(int startYear, int endYear)
        {
            return _repository.ReadAll()
                .Where(p => p.Year >= startYear && p.Year <= endYear);
        }
    }
}

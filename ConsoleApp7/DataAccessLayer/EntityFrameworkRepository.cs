using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model;

namespace DataAccessLayer
{
    public class EntityFrameworkRepository<T> : IRepository<T>, IDisposable where T : class, IDomainObject, new()
    {
        private Context _context;
        private DbSet<T> _dbSet;

        public EntityFrameworkRepository()
        {
            _context = new Context();
            _dbSet = _context.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public IEnumerable<T> ReadAll()
        {
            return new List<T>(_dbSet);
        }

        public T ReadById(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Update(T item)
        {
            var existing = _dbSet.Find(item.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(item);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var item = _dbSet.Find(id);
            if (item != null)
            {
                _dbSet.Remove(item);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
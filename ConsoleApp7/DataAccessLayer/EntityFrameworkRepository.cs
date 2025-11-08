using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model;
//полноценнная орм ситсема
namespace DataAccessLayer
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private Context _context;//отвечает за подключение, отслеживание изменений, сохранение данных крч работа с бдшкой
        private DbSet<T> _dbSet;//сама бдшечка ну типо ее данные

        public EntityFrameworkRepository()
        {
            _context = new Context();//с бдшкой
            _dbSet = _context.Set<T>();// Получаем доступ к таблице для типа T
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();//синхронизируем
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

        //public void Dispose()//очисточка
        //{
        //    _context?.Dispose(); 
        //}
    }
}
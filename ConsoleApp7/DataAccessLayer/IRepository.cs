using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject//тип крч интерфейсик репозиторий где типы имеют ограничение должны           
                                                           //быть реализовывать айдоменчик
    {
        void Add(T item);
        void Delete(int id);
        void Update(T item);
        IEnumerable<T> ReadAll();
        T ReadById(int id);

    }
}
//Репозиторий – класс, что инкапсулирует необходимую для доступа к источником данных логику. 

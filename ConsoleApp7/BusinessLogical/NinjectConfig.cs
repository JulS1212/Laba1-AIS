using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using DataAccessLayer;
using Model;

namespace BusinessLogical
{
    public class NinjectConfig : NinjectModule //класс для настроек Ninject
    {
        public override void Load()
        {
            Bind<IRepository<Painting>>().To<DapperRepository<Painting>>().InSingletonScope(); // настраиваем зависимость (типо Singlenot) IRepository = DapperRepository, т.е одно подключение к бд на весь проект

            Bind<Logic>().ToSelf(); //новый экземпляр при каждом Get<Logic>()
        }
    }
}

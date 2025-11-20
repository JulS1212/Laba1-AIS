using BusinessLogical.Interfaces;
using BusinessLogical.Services;
using BusinessLogical.Validators;
using DataAccessLayer;
using Model;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogical
{
    public class NinjectConfig : NinjectModule //класс для настроек Ninject
    {
        
        public override void Load()
        {
            string сonnectionString = @"Server=DESKTOP-PK8PRRL\SQLEXPRESS;Database=ArtGallery;Trusted_Connection=true;";

            Bind<IRepository<Painting>>().To<DapperRepository<Painting>>().InSingletonScope().WithConstructorArgument("connectionString", сonnectionString); // настраиваем зависимость (типо Singlenot) IRepository = DapperRepository, т.е одно подключение к бд на весь проект

            Bind<IPaintingRepository>().To<PaintingRepository>();

            Bind<IPaintingService>().To<PaintingService>();
            Bind<IPaintingValidator>().To<PaintingValidator>();
        }
    }
}

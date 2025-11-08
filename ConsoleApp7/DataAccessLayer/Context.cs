using System.Data.Entity;
using Model;

namespace DataAccessLayer
{//посредник между бд и классами, описывающими данные
    public class Context : DbContext//сеанс работы с бд в еф
    {
        public Context() : base("ArtGallery")//ищет крч в конфиге
        {

            Database.SetInitializer<Context>(null);//отключает автоматическое создание другой бд
        }

        public DbSet<Painting> Paintings { get; set; }//таблица
    }
}
//Сопоставляет объекты Painting с записями в таблице Paintings
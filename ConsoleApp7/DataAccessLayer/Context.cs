using System.Data.Entity;
using Model;

namespace DataAccessLayer
{
    public class Context : DbContext
    {
        public Context() : base("ArtGallery")
        {
            // ОТКЛЮЧАЕМ создание новой БД - используем существующую
            Database.SetInitializer<Context>(null);
        }

        public DbSet<Painting> Paintings { get; set; }
    }
}
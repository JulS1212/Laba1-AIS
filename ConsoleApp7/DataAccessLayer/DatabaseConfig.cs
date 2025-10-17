using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; } =
            @"Server=(localdb)\MSSQLLocalDB;Database=ArtGallery;Trusted_Connection=true;";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Model;

namespace DataAccessLayer
{
    public class DapperRepository<T>: IRepository<T> where T : class, IDomainObject , new()
    {
        public void Add(T item)
        {
            using (IDbConnection db = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"INSERT INTO Paintings (Title, Artist, Year, Genre) 
                    VALUES (@Title, @Artist, @Year, @Genre)";

                db.Execute(sql, item);
            }
        }

        public void Update(T item)
        {
            using (IDbConnection db = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"UPDATE Paintings 
                    SET Title = @Title, Artist = @Artist, 
                        Year = @Year, Genre = @Genre 
                    WHERE Id = @Id";
                db.Execute(sql, item);
            }
        }

        public void Delete (int id)
        {
            using (IDbConnection db = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = "DELETE FROM Paintings WHERE Id = @Id";
                db.Execute(sql, new { Id = id });
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                return db.Query<T>("SELECT * FROM Paintings");
            }
        }
        public T ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                return db.QueryFirstOrDefault<T>("SELECT * FROM Paintings WHERE Id = @Id",
            new { Id = id });
            }
        }
    }
}

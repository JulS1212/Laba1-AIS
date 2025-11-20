using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Model;
//полуавтоматическая орм система опен р
namespace DataAccessLayer
{
    public class DapperRepository<T>: IRepository<T> where T : class, IDomainObject , new()
    //короче классик даппер может работать разными сущностями разных типов
    //но надо чтобы он реализовывал все методы из интерфейса айрепозиторич
    //но типо вот тип т имеет ограничения должен быть классом реализовывать домэйн обжект и позволяет создавать экзмпляры класса через new
    {
        private readonly string _connectionString;
        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(T item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))//открываем подключение  к бд
            {
                var sql = @"INSERT INTO Paintings (Title, Artist, Year, Genre) 
                    VALUES (@Title, @Artist, @Year, @Genre)";

                db.Execute(sql, item);//типо короче выполнить скл запросик, подставив значения свойств из item
            }
        }

        public void Update(T item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
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
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Paintings WHERE Id = @Id";
                db.Execute(sql, new { Id = id });//запросики скл выполняет
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<T>("SELECT * FROM Paintings");//выполняет запросик и
                                                              ///типо в списочек обьектов типа т преобразует
            }
        }
        public T ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<T>("SELECT * FROM Paintings WHERE Id = @Id",//выполняет скл и типо возращает первое вхождение
                                                                                             //или ниче
            new { Id = id });
            }
        }
    }
}

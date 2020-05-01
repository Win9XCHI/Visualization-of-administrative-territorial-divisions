using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;

public class Input {
    public int Code { get; set; }
    public string PIB { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Rights { get; set; }
}


namespace Handler.Models {
    public interface IUserRepository {
        void Create(Input user);
        void Delete(int id);
        Input Get(int id);
        List<Input> GetUsers();
        void Update(Input user);
    }
    public class UserRepository : IUserRepository {
        string connectionString = null;
        public UserRepository(string conn) {
            connectionString = conn;
        }
        public List<Input> GetUsers() {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                return db.Query<Input>("SELECT * FROM Input").ToList();
            }
        }

        public Input Get(int id) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                return db.Query<Input>("SELECT * FROM Input WHERE Code = @id", new { id }).FirstOrDefault();
            }
        }

        public void Create(Input user) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                var sqlQuery = "INSERT INTO Input (Code, PIB, Login, Password) VALUES(@Code, @PIB, @Login, @Password)";
                db.Execute(sqlQuery, user);

                // если мы хотим получить id добавленного пользователя
                //var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
                //int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                //user.Id = userId.Value;
            }
        }

        public void Update(Input user) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                var sqlQuery = "UPDATE Input SET PIB = @PIB, Login = @Login, Password = @Password WHERE Code = @Id";
                db.Execute(sqlQuery, user);
            }
        }

        public void Delete(int id) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                var sqlQuery = "DELETE FROM Input WHERE Code = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}

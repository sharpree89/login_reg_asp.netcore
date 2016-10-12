using System.Collections.Generic;
using System;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Users.Models;

namespace DapperApp.Factory
{
    public class UserRepository : IFactory<User>
    {
        private string connectionString;
        public UserRepository()
        {
            connectionString = "server=localhost;userid=root;password=root;port=8889;database=loginreg_asp;SslMode=None";
        }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(connectionString);
            }
        }
        public void Create(User user)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES ('{user.first_name}', '{user.last_name}', '{user.email}', '{user.password}', NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
        public IEnumerable<User> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users ORDER BY created_at DESC");
            }
        }
        public User FindByEmail(User user)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE @email = email LIMIT 1", new { email = user.email }).FirstOrDefault();
            }
        }

        // public void update(User current){
        //     using (IDbConnection dbConnection = Connection) {
        //         string query = $"UPDATE users SET likes={current.likes} WHERE id={current.id}";
        //         dbConnection.Open();
        //         dbConnection.Execute(query);
        //     }
        // }
    }
}
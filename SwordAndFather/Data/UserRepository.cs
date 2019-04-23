using SwordAndFather.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        static List<User> _users = new List<User>();
        // static ^^^ means you get one of OR all instances of a class
        // every instance in this class will share the same instance of users

        public User AddUser(string username, string password)
        {
            var newUser = new User(username, password);

            newUser.Id = _users.Count + 1;

            _users.Add(newUser);

            return newUser;
        }

        public List<User> GetAll()
        {
            var users = new List<User>(); // new list which will be used down at the bottom to add user and then return this list
            var connection = new SqlConnection("Server = localhost; Database = SwordAndFather; Trusted_Connection = True;");
            connection.Open(); // Open the Connection

            var getAllUsersCommand = connection.CreateCommand(); // Create the command
            getAllUsersCommand.CommandText = "select * from users";

            var reader = getAllUsersCommand.ExecuteReader(); // Excecute the reader! // if you don't care about the result or there won't be any results, use the ExecuteNonQuery

            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var username = reader["username"].ToString();
                var password = reader["password"].ToString();
                var user = new User(username, password) { Id = id };

                users.Add(user);
            }

            return users;
        }
    }
}

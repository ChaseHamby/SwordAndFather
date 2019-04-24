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
        //static List<User> _users = new List<User>();
        //// static ^^^ means you get one of OR all instances of a class
        //// every instance in this class will share the same instance of users

        const string ConnectionString = "Server = localhost; Database = SwordAndFather; Trusted_Connection = True;";

        public User AddUser(string username, string password)
        {
            //var newUser = new User(username, password);

            //newUser.Id = _users.Count + 1;

            //_users.Add(newUser);

            //return newUser;

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var insertUserCommand = connection.CreateCommand();
                insertUserCommand.CommandText = $@"Insert into Users (username,password)
                                            Output inserted.*
                                            Values(@username,@password)";

                insertUserCommand.Parameters.AddWithValue("username",username);
                insertUserCommand.Parameters.AddWithValue("password", password);

                var reader = insertUserCommand.ExecuteReader();

                if (reader.Read())
                {
                    var insertedPassword = reader["password"].ToString();
                    var insertedUsername = reader["username"].ToString();
                    var insertedId = (int)reader["Id"];

                    var newUser = new User(insertedUsername, insertedPassword) { Id = insertedId };

                    return newUser;
                }
             }

                throw new Exception("No user found");
         }





        public List<User> GetAll()
        {
            var users = new List<User>(); // new list which will be used down at the bottom to add user and then return this list
            var connection = new SqlConnection("Server = localhost; Database = SwordAndFather; Trusted_Connection = True;");
            connection.Open(); // Open the Connection

            var getAllUsersCommand = connection.CreateCommand(); // Create the command to interact with SQL
            getAllUsersCommand.CommandText = @"select username,password,id
                                               from users";

            var reader = getAllUsersCommand.ExecuteReader(); // Excecute the reader! // if you don't care about the result and just want to know how many things were affected, use the ExecuteNonQuery
                                                             // ExecuteScalar for top left value - 1 column / 1 row
            while (reader.Read())
            {
                var id = (int)reader["Id"]; //(int) is there to turn it into an int
                var username = reader["username"].ToString();
                var password = reader["password"].ToString();
                var user = new User(username, password) { Id = id };

                users.Add(user);
            }

            connection.Close(); // Close it down!

            return users;
        }
    }
}

using SwordAndFather.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        const string ConnectionString = "Server = localhost; Database = SwordAndFather; Trusted_Connection = True;";

        public User AddUser(string username, string password)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newUser = db.QueryFirstOrDefault<User>(@"
                    Insert into Users (username,password) 
                    Output inserted.*
                    Values(@username,@password)",
                    new { username, password }); // setting up the parameters required - property needs to match the values above

                if (newUser != null)
                {
                    return newUser;
                }
             }

                throw new Exception("No user created");
         }

        public void DeleteUser(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                // Another way to break this out that is different from updating user 

                var deleteQuery = "Delete from Users where Id = @id";

                var parameter = new { id };

                var rowsAffected = db.Execute(deleteQuery, parameter);

                if (rowsAffected != 1)
                {
                    throw new Exception("Didn't do right");
                }
            }
        }

        public User UpdateUser(User userToUpdate)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var rowsAffected = db.Execute(@"update Users
                           Set username = @username,
                               password = @password
                           Where id = @id", userToUpdate);

                if (rowsAffected == 1)
                    return userToUpdate;
            }
            throw new Exception("Could not update user");
        }

        public IEnumerable<User> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var users = db.Query<User>("select username,password,id from users").ToList();

                var targets = db.Query<Target>("select * from targets").ToList();

                foreach (var user in users)
                {
                    user.Targets = targets.Where(target => target.UserId == user.Id).ToList();
                }

                return users;
            }
        }
    }
}

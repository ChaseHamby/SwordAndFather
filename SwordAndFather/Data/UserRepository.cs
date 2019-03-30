using SwordAndFather.Models;
using System;
using System.Collections.Generic;
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

    }
}

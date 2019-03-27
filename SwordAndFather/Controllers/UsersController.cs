using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwordAndFather.Models;

namespace SwordAndFather.Controllers
{
    [Route("api/[controller]")] // <<< Attribute
    [ApiController] // <<< Attribute - there for the framework to use on how to expose the data
    public class UsersController : ControllerBase // Controller receives an HTTP request and then responds
    {
        static List<User> _users = new List<User>(); // static means you get one of OR all instances of a class - 
            // every instance in this class will share the same instance of users

        [HttpPost("register")] // this means 'api/user/register' - this is the HTTP route that calls the AddUser method below

        public ActionResult<int> AddUser(User newUser) 
        {
            //validation
            if(string.IsNullOrEmpty(newUser.Username)
                || string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest(new {error = "users must have a username and password"});
            }

            newUser.Id = _users.Count + 1;
            _users.Add(newUser);

            return Created($"api/users/{newUser.Id}", newUser);
        }
    }
}
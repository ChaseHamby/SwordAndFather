using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwordAndFather.Data;
using SwordAndFather.Models;

namespace SwordAndFather.Controllers
{
    [Route("api/[controller]")] // <<< Attribute
    [ApiController] // <<< Attribute - there for the framework to use on how to expose the data
    public class UsersController : ControllerBase // Controller receives an HTTP request and then responds
    {
        readonly UserRepository _userRepository;
        readonly CreateUserRequestValidator _validator;

        public UsersController()
        {
            _validator = new CreateUserRequestValidator();
            _userRepository = new UserRepository();
        }

        [HttpPost("register")] // this means 'api/user/register': this is the HTTP route that calls the AddUser method below

        public ActionResult<int> AddUser(CreateUserRequest createRequest) 
        {
            if(!_validator.Validate(createRequest))
            {
                return BadRequest(new {error = "users must have a username and password"});
            }

            var newUser = _userRepository.AddUser(createRequest.Username, createRequest.Password);

            // http response
            return Created($"api/users/{newUser.Id}", newUser);
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();

            return Ok(users);
        }
    }

}
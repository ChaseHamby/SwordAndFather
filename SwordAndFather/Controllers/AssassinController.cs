using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwordAndFather.Models;

namespace SwordAndFather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssassinController : ControllerBase
    {

        readonly CreateAssassinRequestValidator _validator; // <<<< - fields are class level variables //
            // ^^^ by doing _varibale as a field, it makes it accessible across the whole class to be used
        readonly AssassinRepository _repository; 

        public AssassinController()
        {
            _validator = new CreateAssassinRequestValidator();
            _repository = new AssassinRepository();
        }

        [HttpPost("register")]

        public ActionResult AddAssassin(CreateAssassinRequest request)
        {
            if (!_validator.Validate(request))
            {
                return BadRequest();
            }

            var newAssassin = _repository.AddAssassin(request.Catchphrase, request.Catchphrase, request.PreferredWeapon);

            return Created($"api/assassins/{newAssassin.Id}", newAssassin);
        }
    }

    public class AssassinRepository
    {
        static readonly List<Assassin> Assassins = new List<Assassin>();

        public Assassin AddAssassin(string codeName, string catchphrase, string preferredWeapon)
        {
            var newAssassin = new Assassin(codeName, catchphrase, preferredWeapon);

            newAssassin.Id = Assassins.Count + 1;

            Assassins.Add(newAssassin);

            return newAssassin;
        }
    }

    public class CreateAssassinRequestValidator
    {
        public bool Validate(CreateAssassinRequest request)
        {
            return !string.IsNullOrEmpty(request.Catchphrase) &&
                   !string.IsNullOrEmpty(request.CodeName) &&
                   !string.IsNullOrEmpty(request.PreferredWeapon);
        }
    }
}
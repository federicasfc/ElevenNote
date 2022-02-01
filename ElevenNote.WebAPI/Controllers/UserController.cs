using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase //inheritance, yes; ControllerBase actually has the logic that catches our HTTP requests, or has stuff like ModelState built-in; basically handles the background functionality stuff of http that we use. I think.
    {
        private readonly IUserService _service; //equivalent of _context?; note interface is being used, not class

        //Constructor
        public UserController(IUserService service)
        {
            _service = service; //remember service is where the bulk of the logic(and methods reside) that's why we're utilizing it here (creo)
        }

        //Post 

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _service.RegisterUserAsync(model);
            if (!registerResult)
            {
                return BadRequest("User could not be registered");
            }
            return Ok("User was registered"); //bringing in service here in the way that previously we would have used context. If I'm following, we are basically adding an extra layer between our database and the userInput data. 
        } //works, should ask about switched order just in case

        //GetUserById

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            var userDetail = await _service.GetUserByIdAsync(userId);

            if (userDetail is null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }
    }
}
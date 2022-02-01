using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.Token;
using ElevenNote.Models.User;
using ElevenNote.Services.Token;
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
        private readonly IUserService _userService; //equivalent of _context?; note interface is being used, not class

        private readonly TokenService _tokenService;

        //Constructor
        public UserController(IUserService userService, TokenService tokenService)
        {
            _userService = userService; //remember service is where the bulk of the logic(and methods reside) that's why we're utilizing it here (creo)
            _tokenService = tokenService;
        }

        //Post 

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
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
            var userDetail = await _userService.GetUserByIdAsync(userId);

            if (userDetail is null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }

        [HttpPost("~/api/Token")]

        public async Task<IActionResult> Token([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tokenResponse = await _tokenService.GetTokenAsync(request);
            if (tokenResponse is null)
                return BadRequest("Invalid username or password");

            return Ok(tokenResponse);
        }
    }
}
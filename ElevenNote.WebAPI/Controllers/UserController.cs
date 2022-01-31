using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase //inheritance?
    {
        private readonly IUserService _service; //equivalent of _context?; note interface is being used, not class

        //Constructor
        public UserController(IUserService service)
        {
            _service = service;
        }
    }
}
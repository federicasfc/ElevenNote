using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.Note;
using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        //Field with access to Service methods
        private readonly INoteService _noteService;

        //Constructor
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        //Post api/Note
        [HttpPost]

        public async Task<IActionResult> CreateNote([FromBody] NoteCreate request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _noteService.CreateNoteAsync(request) == false)
                return BadRequest("Note could not be created."); //switched order of this to follow pattern of bad result first; may change working, in which case remove == false, and switch OK and BadRequest


            return Ok("Note created successfully");
        }

        //GetAllNotes Endpoint

        [HttpGet]

        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        } //works
    }
}
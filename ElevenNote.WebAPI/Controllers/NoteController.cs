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
        } //works

        //GetAllNotes Endpoint

        [HttpGet]

        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        } //works

        //GetNoteById Endpoint

        [HttpGet("{noteId:int}")]
        public async Task<IActionResult> GetNoteById([FromRoute] int noteId)
        {
            var detail = await _noteService.GetNoteByIdAsync(noteId);

            //Similar to our service method, we're using a ternary to determine our return type

            //If the returned value (detail) is not null, return it with a 200 Ok
            //Otherwise return a NotFound() 404 response

            return detail is null
            ? NotFound()
            : Ok(detail); //switched logic again to have good result at bottom, makes less sense because now it is technically the else, but I want to keep the order anyway; confirm that I can do this

        }




    }
}
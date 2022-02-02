using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.Note;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly int _userId;
        private readonly ApplicationDbContext _dbContext;

        //Constructor

        public NoteService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            var userClaims = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var value = userClaims.FindFirst("Id")?.Value;
            //validating claim value?:
            var validId = int.TryParse(value, out _userId);
            if (!validId)
                throw new Exception("Attempted to build NoteService without User Id claim");

            _dbContext = dbContext;
        } //Mod 15.04

        //CreateNote

        public async Task<bool> CreateNoteAsync(NoteCreate request)
        {
            var noteEntity = new NoteEntity
            {
                Title = request.Title,
                Content = request.Content,
                CreatedUtc = DateTimeOffset.Now,
                OwnerId = _userId

            };

            _dbContext.Notes.Add(noteEntity);

            var numberOfChanges = await _dbContext.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        //GetAllNotes

        public async Task<IEnumerable<NoteListItem>> GetAllNotesAsync()
        {
            var notes = await _dbContext.Notes
                .Where(entity => entity.OwnerId == _userId)
                .Select(entity => new NoteListItem
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    CreatedUtc = entity.CreatedUtc
                }).ToListAsync();
            return notes;
        }

        //GetNoteById

        public async Task<NoteDetail> GetNoteByIdAsync(int noteId)
        {
            //Find the first note that has the given Id and an OwnerId that matches the requesting userId

            var noteEntity = await _dbContext.Notes
                .FirstOrDefaultAsync(e => e.Id == noteId && e.OwnerId == _userId);

            //If noteEntity is null then return null, otherwise initialize and return a new NoteDetail

            return noteEntity is null ? null : new NoteDetail
            {
                Id = noteEntity.Id,
                Title = noteEntity.Title,
                Content = noteEntity.Content,
                CreatedUtc = noteEntity.CreatedUtc,
                ModifiedUtc = noteEntity.ModifiedUtc
            };

        }

        //UpdateNote

        public async Task<bool> UpdateNoteAsync(NoteUpdate request)
        {
            //Find the note and validate it's owned by the user 

            var noteEntity = await _dbContext.Notes.FindAsync(request.Id);

            //By using the null conditional operator we can check if it's null at the same time we check the OwnerId
            if (noteEntity?.OwnerId != _userId)
                return false;

            //Now we update the entity's properties
            noteEntity.Title = request.Title;
            noteEntity.Content = request.Content;
            noteEntity.ModifiedUtc = DateTimeOffset.Now;

            //Save the changes to the database and capture how many rows were updated

            var numberOfChanges = await _dbContext.SaveChangesAsync();

            //numberOfChanges is stated to be equal to 1 because only one row is updated

            return numberOfChanges == 1;



        } //Mod 20.02

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            //Find the note by the given Id

            var noteEntity = await _dbContext.Notes.FindAsync(noteId);

            //Validate the note exists and is owned by the user
            if (noteEntity?.OwnerId != _userId)
                return false;

            //Remove the note from the DbContet and assert that the one change was saved

            _dbContext.Notes.Remove(noteEntity);
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}
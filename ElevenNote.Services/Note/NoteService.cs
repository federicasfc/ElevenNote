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
    }
}
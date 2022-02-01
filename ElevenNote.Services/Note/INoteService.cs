using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.Note;

namespace ElevenNote.Services.Note
{
    public interface INoteService
    {
        Task<IEnumerable<NoteListItem>> GetAllNotesAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Models.Note
{
    public class NoteListItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTimeOffset CreatedUtc { get; set; }
    }
}
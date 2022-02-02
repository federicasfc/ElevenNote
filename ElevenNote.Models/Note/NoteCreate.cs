using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Models.Note
{
    public class NoteCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} characters long.")]
        public string Title { get; set; }

        [Required]
        [MaxLength(8000, ErrorMessage = "{0} must contain no more than {1} characters.")]
        public string Content { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Models.User
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; }

    } //don't need validation attributes here because by virtue of being part of out db, they are already validated(had to meet the specifications at some point)
}
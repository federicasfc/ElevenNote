using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.User;

namespace ElevenNote.Services.User
{
    public interface IUserService
    {
        //RegisterUser method signature
        Task<bool> RegisterUserAsync(UserRegister model);

        //GetUserById method signature

        Task<UserDetail> GetUserByIdAsync(int userId);


    }
}
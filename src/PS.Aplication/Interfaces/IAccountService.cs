using PS.Application.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> UserExists(string userName);
        Task<UserExistingDto> GetUserByUserNameAsync(string userName);
        Task<SignInResult> CheckUserPasswordAsync(UserLoginDto userLogin);
        Task<UserExistingDto> CreateAccountAsync(UserDto newUser);
        Task<UserExistingDto> UpdateAccount(int id, UserExistingDto userExisting);
    }
}

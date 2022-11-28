using AutoMapper;
using PS.Application.Dtos;
using PS.Application.Interfaces;
using PS.Domain.Entities.Identity;
using PS.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserPersist _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserPersist userPersist)
        {
            _userRepository = userPersist;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserLoginDto userLogin)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.NormalizedUserName == userLogin.UserName.ToUpper());
                if (user != null)
                {
                    //no 3º parametro é possivel bloquear o usuario caso a senha esteja invalida
                    return await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
                }
                return new SignInResult();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserExistingDto> CreateAccountAsync(UserDto newUser)
        {
            try
            {
                User user = _mapper.Map<User>(newUser);

                IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    return _mapper.Map<UserExistingDto>(user);
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserExistingDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(userName);

                if (user != null)
                {
                    return _mapper.Map<UserExistingDto>(user);
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserExistingDto> UpdateAccount(int id, UserExistingDto userExisting)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null) return null;

                userExisting.Id = user.Id;

                _mapper.Map(userExisting, user);
                if (!string.IsNullOrEmpty(userExisting.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userExisting.Password);
                }
                _userRepository.Update(user);

                if (await _userRepository.SaveChangesAsync())
                {
                    var upUser = await _userRepository.GetUserByIdAsync(user.Id);
                    var mapRes = _mapper.Map<UserExistingDto>(upUser);
                    return mapRes;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(q => q.NormalizedUserName == userName.ToUpper());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

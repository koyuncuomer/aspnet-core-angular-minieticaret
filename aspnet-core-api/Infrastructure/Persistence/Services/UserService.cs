using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Services;
using Application.DTOs.User;
using Application.Exceptions;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserRequestDto model)
        {
            var result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                NameSurname = model.NameSurname
            }, model.Password);

            if (result.Succeeded)
                return new() { Succeeded = true, Message = "User created succesfully." };

            throw new UserCreateFailedException(result.Errors.Select(e => e.Description));
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenEndDate, int refreshTokenLifeTime)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenEndDate.AddSeconds(refreshTokenLifeTime);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();

        }
    }
}

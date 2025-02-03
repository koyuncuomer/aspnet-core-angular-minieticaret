using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.User;
using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateAsync(CreateUserRequestDto model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenEndDate, int refreshTokenLifeTime);
    }
}

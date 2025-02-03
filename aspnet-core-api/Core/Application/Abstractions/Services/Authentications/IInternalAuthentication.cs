using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<TokenDto> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
        Task<TokenDto> RefreshTokenLoginAsync(string refreshToken);
    }
}

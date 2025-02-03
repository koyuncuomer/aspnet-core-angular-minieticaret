using Application.Abstractions.Services;
using Application.Abstractions.Token;
using Application.DTOs;
using Application.Exceptions;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;
        readonly IUserService _userService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
        }

        public async Task<TokenDto> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null)
                throw new NotFoundUserException();

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                TokenDto token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }

            throw new AuthenticationErrorException();
        }

        public async Task<TokenDto> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                TokenDto token = _tokenHandler.CreateAccessToken(15);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }
            else
                throw new NotFoundUserException();

        }
    }
}

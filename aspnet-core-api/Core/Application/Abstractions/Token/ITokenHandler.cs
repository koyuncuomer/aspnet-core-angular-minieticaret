using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDto CreateAccessToken(int seconds);
        string CreateRefreshToken();
    }
}

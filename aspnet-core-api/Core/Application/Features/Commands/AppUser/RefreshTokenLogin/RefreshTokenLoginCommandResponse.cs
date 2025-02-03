using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandResponse
    {
        public TokenDto Token { get; set; }
    }
}

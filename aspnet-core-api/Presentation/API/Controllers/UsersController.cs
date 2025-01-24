using Application.Exceptions;
using Application.Features.Commands.AppUser.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            try
            {
                var response = await _mediator.Send(createUserCommandRequest);
                return StatusCode(201, response);
            }
            catch (UserCreateFailedException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Errors = ex.Errors
                });
            }
        }


    }
}

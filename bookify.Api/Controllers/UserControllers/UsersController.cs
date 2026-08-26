using Bookify.Application.users.LogInUser;
using Bookify.Application.users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookify.Api.Controllers.UserControllers
{
    [ApiController]
    [Route("api/users")]

    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var RegisterUserCommand = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.PassWord);
            var result = await _sender.Send(RegisterUserCommand, cancellationToken);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var LogInUserCommand = new LogInCommand(request.Email, request.PassWord);
            var result = await _sender.Send(LogInUserCommand, cancellationToken);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }
            return Ok(result.Value);
        }
    }


}

using Bookify.Application.users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookify.Api.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUsersController : ControllerBase
    {
        private readonly ISender _sender;

        public RegisterUsersController(ISender sender)
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

    }
}

using bookify.domain.Abstractions;
using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.users.LogInUser
{
    internal sealed class LogInCommandHandler : ICommandHandler<LogInCommand, AccessTokenResponse>
    {
        private readonly IJwTService _jwTService;

        public LogInCommandHandler(IJwTService jwTService)
        {
            _jwTService = jwTService;
        }

        public async Task<Result<AccessTokenResponse>> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            var result = await _jwTService.GetAccessTokenAsync(request.Email, request.Password, cancellationToken);

            if (result.IsFailure)
            {
                return Result.Failure<AccessTokenResponse>(result.Error);
            }
            return new AccessTokenResponse(result.Value);
        }
    }
}

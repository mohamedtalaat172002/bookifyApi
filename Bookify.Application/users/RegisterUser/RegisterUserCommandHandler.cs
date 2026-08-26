using bookify.domain.Abstractions;
using bookify.domain.Users;
using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.users.RegisterUser
{
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly IUniteOfWork _uniteOfWork;

        public RegisterUserCommandHandler(IAuthenticationService authenticationService, IUserRepository userRepository, IUniteOfWork uniteOfWork)
        {
            this._authenticationService = authenticationService;
            this._userRepository = userRepository;
            this._uniteOfWork = uniteOfWork;
        }

        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var user = User.Create(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email));

            var IdentityId = await _authenticationService.RegisterAsync
                (user, request.Password, cancellationToken);

            user.SetIdentiyId(IdentityId);
            _userRepository.Add(user);
            await _uniteOfWork.SaveChangesAsync();
            return user.id;
        }
    }
}

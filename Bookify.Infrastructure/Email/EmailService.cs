using Bookify.Application.Abstraction.EmailService;

namespace Bookify.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendAsync(bookify.domain.Users.Email recipient, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}

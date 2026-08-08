using bookify.domain.Users;

namespace Bookify.Application.Abstraction.EmailService
{
    public interface IEmailService
    {
        Task SendAsync(Email recipient, string subject, string body);
    }
}

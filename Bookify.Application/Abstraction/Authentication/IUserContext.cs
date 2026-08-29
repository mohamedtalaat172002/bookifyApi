namespace Bookify.Application.Abstraction.Authentication
{
    public interface IUserContext
    {
        Guid UserId { get; }
        string IdentityId { get; }
    }
}

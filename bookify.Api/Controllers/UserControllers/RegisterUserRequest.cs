namespace bookify.Api.Controllers.UserControllers
{
    public sealed record RegisterUserRequest
        (
        string Email,
        string FirstName,
        string LastName,
        string PassWord
        );

}

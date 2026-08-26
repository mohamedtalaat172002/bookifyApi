namespace bookify.Api.Controllers.UserControllers
{
    public sealed record LoginUserRequest(
        string Email,
        string PassWord
        );
}
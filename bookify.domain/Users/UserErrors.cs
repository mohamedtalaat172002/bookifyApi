using bookify.domain.Abstractions;

namespace bookify.domain.Users
{
    public static class UserErrors
    {
        public static Error NotFound = new("user.Notfound", "User With The Specified Identifier not found.");

    }
}

namespace bookify.domain.Users
{
    public sealed class Permission
    {
        public static readonly Permission UserRead = new Permission(1, "User:Read");
        public Permission(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; init; }
        public string Name { get; init; }
    }
}

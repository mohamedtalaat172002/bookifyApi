namespace bookify.domain.Users
{
    public sealed class Role
    {
        public static readonly Role Registered = new Role(1, "registerd");
        public Role(int Id, string name)
        {
            this.Id = Id;
            this.name = name;
        }

        public int Id { get; init; }
        public string name { get; init; } = string.Empty;

        public ICollection<User> users { get; init; } = new List<User>();
    }
}

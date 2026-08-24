using bookify.domain.Abstractions;
using bookify.domain.Users.Events;

namespace bookify.domain.Users
{
    public sealed class User : Entity
    {
        private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        private User()
        {


        }
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }
        public string IdentityID { get; private set; } = string.Empty;

        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvents(userId: user.id));
            return user;
        }

        public void SetIdentiyId(string IdentiyId)
        {
            IdentityID = IdentiyId;
        }

    }
}

using bookify.domain.Users;

namespace Bookify.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<User>, IUserRepository

    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public override void Add(User entity)
        {

            foreach (var role in entity.Roles)
            {

                _applicationDbContext.Attach(role);
            }
            _applicationDbContext.Add(entity);
        }



    }
}

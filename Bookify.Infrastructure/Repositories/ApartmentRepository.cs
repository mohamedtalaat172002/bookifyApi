using bookify.domain.Apartments;

namespace Bookify.Infrastructure.Repositories
{
    internal sealed class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }


    }
}

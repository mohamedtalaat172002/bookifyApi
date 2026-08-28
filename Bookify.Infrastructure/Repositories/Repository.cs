using bookify.domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories
{
    internal abstract class Repository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _applicationDbContext;

        protected Repository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Set<T>().FirstOrDefaultAsync(entity => entity.id == id, cancellationToken);

        }
        public virtual void Add(T entity)
        {
            _applicationDbContext.Add(entity);
        }
    }
}

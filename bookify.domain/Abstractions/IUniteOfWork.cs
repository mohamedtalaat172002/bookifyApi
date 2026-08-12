namespace bookify.domain.Abstractions
{
    public interface IUniteOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

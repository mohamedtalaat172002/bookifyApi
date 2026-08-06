namespace bookify.domain.Abstractions
{
    public interface IUniteOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Apartments.SearchApartments
{
    public record class SearchApartmentsQuery(DateOnly startDate, DateOnly endDate) : IQuery<IReadOnlyList<ApartmentResponse>>
    {
    }
}

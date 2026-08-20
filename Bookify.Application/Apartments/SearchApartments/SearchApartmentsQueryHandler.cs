using bookify.domain.Abstractions;
using bookify.domain.Bookings;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Messaging;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartments
{
    internal sealed class SearchApartmentsQueryHandler : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private static readonly int[] ActiveBookingStatuses =
            {
                (int)BookingStatus.reserved,
                (int)BookingStatus.confirmed,
                (int)BookingStatus.completed
            };

        public SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    a.id AS Id,
                    a.Name AS Name,
                    a.Description AS Description,
                    a.Price_Amount AS Price,
                    a.Price_Currency AS Currency,
                    a.Address_Country AS Country,
                    a.Address_ZipCode AS ZipCode,
                    a.Address_City AS City,
                    a.Address_Street AS Street
                FROM Apartments AS a
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM Bookings AS b
                    WHERE
                        b.ApartmentId = a.id AND
                        b.Duration_Start <= @EndDate AND
                        b.Duration_End >= @StartDate AND
                        b.Status IN @ActiveBookingStatuses
                )
                """;

            var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>
                 (
                     sql,
                     (apartment, address) =>
                     {
                         apartment.Address = address;
                         return apartment;
                     },
                     new
                     {
                         StartDate = request.startDate,
                         EndDate = request.endDate,
                         ActiveBookingStatuses
                     },
                     splitOn: "Country"
                 );

            return apartments.ToList();
        }
    }
}
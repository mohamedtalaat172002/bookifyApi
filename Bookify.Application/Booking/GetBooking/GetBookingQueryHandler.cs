using bookify.domain.Abstractions;
using bookify.domain.Bookings;
using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Messaging;
using Dapper;

namespace Bookify.Application.Booking.GetBooking
{
    internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IUserContext _userContext;
        public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _userContext = userContext;
        }
        public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = """
            SELECT
                id AS Id,
                apartment_id AS ApartmentId,
                user_id AS UserId,
                status AS Status,
                price_for_period_amount AS PriceAmount,
                price_for_period_currency AS PriceCurrency,
                cleaning_fee_amount AS CleaningFeeAmount,
                cleaning_fee_currency AS CleaningFeeCurrency,
                amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                amenities_up_charge_currency AS AmenitiesUpChargeCurrency,
                total_price_amount AS TotalPriceAmount,
                total_price_currency AS TotalPriceCurrency,
                duration_start AS DurationStart,
                duration_end AS DurationEnd,
                created_on_utc AS CreatedOnUtc
            FROM bookings
            WHERE id = @BookingId
            """;

            var booking = await connection.QuerySingleOrDefaultAsync<BookingResponse>
                (sql, new { request.id });
            if (booking == null || booking.UserId != _userContext.UserId)
            {
                return Result.Failure<BookingResponse>(BookingErrors.NotFound);
            }
            return booking;
        }
    }
}

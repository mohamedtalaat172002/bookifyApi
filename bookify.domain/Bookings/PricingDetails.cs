using bookify.domain.Shared;

namespace bookify.domain.Bookings
{
    public record PricingDetails
    (
        Money PriceForPeriod,
        Money CleaningFee,
        Money AmenetiesUpCharge,
        Money TotalPrice
    );
}

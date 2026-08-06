using bookify.domain.Apartments;
using bookify.domain.Shared;

namespace bookify.domain.Bookings
{
    public class PricingService
    {
        public PricingDetails CalculatePricing(Apartment apartment, DateRange dateRange)
        {

            var currency = apartment.Price.Currency;
            var priceForPeriod = new Money(apartment.Price.Amount * dateRange.LengthInDays, currency);
            decimal AmenetiesUpChargePercintage = 0;
            foreach (var amenity in apartment.Amenities)
            {
                AmenetiesUpChargePercintage += amenity switch
                {
                    Amenity.GardenView or Amenity.MountainView => .05m,
                    Amenity.AirConditioning => .01m,
                    Amenity.Parking => .01m,
                    _ => 0
                };
            }
            Money amenetiesUpCharge = Money.Zero();
            if (AmenetiesUpChargePercintage > 0)
            {
                amenetiesUpCharge = new Money(priceForPeriod.Amount * AmenetiesUpChargePercintage, currency);
            }
            Money TotalPeice = new Money(priceForPeriod.Amount + amenetiesUpCharge.Amount, currency);

            if (!apartment.CleaningFee.IsZero())
            {
                TotalPeice = new Money(TotalPeice.Amount + apartment.CleaningFee.Amount, currency);
            }

            return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenetiesUpCharge, TotalPeice);
        }

    }
}

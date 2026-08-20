using bookify.domain.Abstractions;
using bookify.domain.Apartments;
using bookify.domain.Bookings.Events;
using bookify.domain.Shared;

namespace bookify.domain.Bookings
{
    public sealed class Booking : Entity
    {

        public Booking(Guid id, Guid userId, Guid apartmentId, DateRange duration, Money priceForPeriod, Money cleaningFee, Money amenetiesUpCharge, Money totalPrice, BookingStatus status) : base(id)
        {
            UserId = userId;
            ApartmentId = apartmentId;
            Duration = duration;
            PriceForPeriod = priceForPeriod;
            CleaningFee = cleaningFee;
            AmenetiesUpCharge = amenetiesUpCharge;
            TotalPrice = totalPrice;
            Status = status;
        }

        public Guid UserId { get; private set; }
        public Guid ApartmentId { get; private set; }
        public DateRange Duration { get; private set; }
        public Money PriceForPeriod { get; private set; }
        public Money CleaningFee { get; private set; }
        public Money AmenetiesUpCharge { get; private set; }
        public Money TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime? CreatedOnUtc { get; private set; }
        public DateTime? ConfirmedOnUtc { get; private set; }
        public DateTime? RejectedOnUtc { get; private set; }
        public DateTime? CancelledOnUtc { get; private set; }
        public DateTime? CompletedOnUtc { get; private set; }


        public static Booking Reserve(PricingService pricingService,
          Apartment apartmnet, Guid UserId, DateRange duration, DateTime utcNow)
        {
            PricingDetails pricingDetails = pricingService.CalculatePricing(apartmnet, duration);

            var booking = new Booking(Guid.NewGuid(),
                UserId,
                apartmnet.id,
                duration,
                pricingDetails.PriceForPeriod,
                pricingDetails.CleaningFee,
                pricingDetails.AmenetiesUpCharge,
                pricingDetails.TotalPrice,
                BookingStatus.reserved);
            booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.id));
            apartmnet.LastBookedOnUtc = utcNow;
            return booking;
        }

        private Booking()
        {

        }
        public Result Confirm(DateTime utcNow)
        {
            if (Status != BookingStatus.reserved)
            {
                return Result.Failure(BookingErrors.NotReserved);
            }

            Status = BookingStatus.confirmed;
            ConfirmedOnUtc = utcNow;

            RaiseDomainEvent(new BookingConfirmedDomainEvent(id));

            return Result.Success();
        }

        public Result Reject(DateTime utcNow)
        {
            if (Status != BookingStatus.reserved)
            {
                return Result.Failure(BookingErrors.NotReserved);
            }

            Status = BookingStatus.rejected;
            RejectedOnUtc = utcNow;

            RaiseDomainEvent(new BookingRejectedDomainEvent(id));

            return Result.Success();
        }

        public Result Complete(DateTime utcNow)
        {
            if (Status != BookingStatus.confirmed)
            {
                return Result.Failure(BookingErrors.NotConfirmed);
            }

            Status = BookingStatus.completed;
            CompletedOnUtc = utcNow;

            RaiseDomainEvent(new BookingCompletedDomainEvent(id));

            return Result.Success();
        }

        public Result Cancel(DateTime utcNow)
        {
            if (Status != BookingStatus.confirmed)
            {
                return Result.Failure(BookingErrors.NotConfirmed);
            }

            var currentDate = DateOnly.FromDateTime(utcNow);

            if (currentDate > Duration.Start)
            {
                return Result.Failure(BookingErrors.AlreadyStarted);
            }

            Status = BookingStatus.cancelled;
            CancelledOnUtc = utcNow;

            RaiseDomainEvent(new BookingCancelledDomainEvent(id));

            return Result.Success();
        }

    }
}

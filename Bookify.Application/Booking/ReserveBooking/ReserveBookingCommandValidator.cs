using FluentValidation;

namespace Bookify.Application.Booking.ReserveBooking
{
    public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(v => v.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(v => v.ApartmentId).NotEmpty().WithMessage("ApartmentId is required.");
            RuleFor(v => v.StartDate).LessThan(v => v.EndDate);


        }
    }
}

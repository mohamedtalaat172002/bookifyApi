using bookify.domain.Bookings;
using bookify.domain.Bookings.Events;
using bookify.domain.Users;
using Bookify.Application.Abstraction.EmailService;
using MediatR;

namespace Bookify.Application.Booking.ReserveBooking
{
    internal sealed class ReserveBookingDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        public ReserveBookingDomainEventHandler(IEmailService emailService, IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _emailService = emailService;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }
        public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
        {

            var booking = await _bookingRepository.GetByIdAsync(notification.id, cancellationToken);
            if (booking == null) { return; }
            var user = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);
            if (user == null) { return; }
            await _emailService.SendAsync(
                  user.Email,
                  "Booking Reserved",
                  $"You have 10 minutes to confirm your booking."
              );

        }
    }
}

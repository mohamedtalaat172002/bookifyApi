using bookify.domain.Abstractions;
using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Users;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Exceptions;

namespace Bookify.Application.Booking.ReserveBooking
{
    internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IUniteOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly PricingService _pricingService;

        private ReserveBookingCommandHandler(IUniteOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IBookingRepository bookingRepository, IUserRepository userRepository, IApartmentRepository apartmentRepository, PricingService pricingService)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _pricingService = pricingService;
        }
        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }
            var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
            if (apartment == null)
            {
                return Result.Failure<Guid>(ApartmentErrors.NotFound);
            }
            var duration = DateRange.Create(request.StartDate, request.EndDate);

            if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
            {
                return Result.Failure<Guid>(BookingErrors.OverLap);
            }
            try
            {
                var booking = bookify.domain.Bookings.Booking.Reserve(_pricingService, apartment, request.UserId, duration, _dateTimeProvider.UtcNow);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return booking.id;
            }
            catch (ConcurrencyException)
            { return Result.Failure<Guid>(BookingErrors.OverLap); }


        }
    }
}

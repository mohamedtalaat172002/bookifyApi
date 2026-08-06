using bookify.domain.Abstractions;

namespace bookify.domain.Bookings
{
    public static class BookingErrors
    {

        public static Error NotFound = new("booking.Notfound", "Booking With The Specified Identifier not found.");
        public static Error NotReserved = new("booking.NotReserved", "Booking Is Not Pending.");
        public static Error NotConfirmed = new("booking.NotConfirmed", "Booking Is Not Reserved.");

        public static Error AlreadyStarted = new("booking.AlreadyStarted", "Booking Already started.");

        public static Error OverLap = new("booking.OverLap", "Booking Is OverLapping With An Existing One.");


    }
}

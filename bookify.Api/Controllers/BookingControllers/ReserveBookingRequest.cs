namespace bookify.Api.Controllers.BookingControllers
{
    public record ReserveBookingRequest(Guid aprtmentId, Guid UserId, DateOnly StartDate, DateOnly EndDate)
    {
    }
}

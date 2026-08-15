using Bookify.Application.Booking.GetBooking;
using Bookify.Application.Booking.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bookify.Api.Controllers.BookingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveBookingController : ControllerBase
    {
        private readonly ISender _sender;

        public ReserveBookingController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetBookingQuery(id), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ReserveBooking(ReserveBookingRequest request, CancellationToken cancellationToken)
        {

            var result = await _sender.Send(new ReserveBookingCommand(request.aprtmentId, request.UserId, request.StartDate, request.EndDate), cancellationToken);

            return result.IsFailure ? BadRequest(result.Error) : CreatedAtAction(nameof(GetBookingById), new { id = result.Value }, result.Value);
        }
    }
}

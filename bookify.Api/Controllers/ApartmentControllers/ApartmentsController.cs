using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bookify.Api.Controllers.ApartmentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISender _sender;

        public ApartmentsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> SearchApartments(DateOnly start, DateOnly end, CancellationToken cancellationToken)
        {

            var result = await _sender.Send(new SearchApartmentsQuery(start, end), cancellationToken);
            return Ok(result.Value);
        }
    }
}

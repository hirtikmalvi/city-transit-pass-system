using CTPS.API.DTOs.Trip;
using CTPS.API.Services.Implementations;
using CTPS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTPS.API.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripService tripService;

        public TripsController(ITripService _tripService)
        {
            tripService = _tripService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTrips([FromRoute] int userId)
        {
            var result = await tripService.GetTripsByUser(userId, null);
            return Ok(result);
        }

        [HttpGet("user/{userId}/filter")]
        public async Task<ActionResult> GetTripsFiltered(
        int userId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
        {
            var filter = new TripFilterRequestDTO { From = from, To = to };
            var result = await tripService.GetTripsByUser(userId, filter);
            return Ok(result);
        }

        [HttpGet("trips")]
        public async Task<ActionResult> GetMyTrips(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
        {
            var myUserId = 1; // temporary
            var filter = new TripFilterRequestDTO { From = from, To = to };
            var result = await tripService.GetTripsByUser(myUserId, filter);
            return Ok(result);
        }
    }
}

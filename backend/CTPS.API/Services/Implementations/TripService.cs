using CTPS.API.Common;
using CTPS.API.DTOs.Trip;
using CTPS.API.Repositories.Interfaces;
using CTPS.API.Services.Interfaces;

namespace CTPS.API.Services.Implementations
{
    public class TripService : ITripService
    {
        private readonly ITripsRepository tripsRepository;
        public TripService(ITripsRepository _tripsRepository)
        {
            tripsRepository = _tripsRepository;
        }
        public async Task<Result<List<TripResponseDTO>>> GetTripsByUser(int userId, TripFilterRequestDTO? filter)
        {
            var trips = await tripsRepository.GetTripsByUser(userId, filter);
            return Result<List<TripResponseDTO>>.Ok(trips, 200);
        }
    }
}

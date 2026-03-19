using CTPS.API.Common;
using CTPS.API.DTOs.Trip;

namespace CTPS.API.Services.Interfaces
{
    public interface ITripService
    {
        Task<Result<List<TripResponseDTO>>> GetTripsByUser(int userId, TripFilterRequestDTO? filter);
    }
}

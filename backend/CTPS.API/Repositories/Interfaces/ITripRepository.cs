using CTPS.API.DTOs.Trip;
using CTPS.API.Models;

namespace CTPS.API.Repositories.Interfaces
{
    public interface ITripsRepository
    {
        Task AddTrip(Trip trip);
        Task<List<TripResponseDTO>> GetTripsByUser(int userId, TripFilterRequestDTO? filter);
    }
}

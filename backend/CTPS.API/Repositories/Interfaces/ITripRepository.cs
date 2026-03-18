using CTPS.API.Models;

namespace CTPS.API.Repositories.Interfaces
{
    public interface ITripsRepository
    {
        Task AddTrip(Trip trip);
    }
}

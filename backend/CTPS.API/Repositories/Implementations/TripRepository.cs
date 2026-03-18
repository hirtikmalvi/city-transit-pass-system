using CTPS.API.Data;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;

namespace CTPS.API.Repositories.Implementations
{
    public class TripRepository : ITripsRepository
    {
        private readonly AppDbContext context;
        public TripRepository(AppDbContext _context)
        {
            context = _context;
        }

        public async Task AddTrip(Trip trip)
        {
            context.Add(trip);
            await context.SaveChangesAsync();
        }
    }
}

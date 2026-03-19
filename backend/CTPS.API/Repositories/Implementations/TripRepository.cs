using CTPS.API.Data;
using CTPS.API.DTOs.Trip;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<TripResponseDTO>> GetTripsByUser(int userId, TripFilterRequestDTO? filter)
        {
            var query = context.Trips
            .Include(t => t.UserPass)
                .ThenInclude(up => up!.PassType)
            .Include(t => t.ValidatedByNavigation)
            .Where(t => t.UserPass != null && t.UserPass.UserId == userId);

            // Apply date filters if provided
            if (filter?.From.HasValue == true)
                query = query.Where(t => t.ValidatedAt >= filter.From.Value);

            if (filter?.To.HasValue == true)
                query = query.Where(t => t.ValidatedAt <= filter.To.Value);

            var trips = await query
            .OrderByDescending(t => t.ValidatedAt)
            .ToListAsync();

            return trips.Select(t => new TripResponseDTO
            {
                Id = t.Id,
                PassCode = t.UserPass!.PassCode,
                PassTypeName = t.UserPass.PassType!.Name,
                TransportMode = t.TransportMode,
                RouteInfo = t.RouteInfo,
                ValidatedAt = t.ValidatedAt,
                ValidatedByName = t.ValidatedByNavigation?.Name
            }).ToList();
        }
    }
}

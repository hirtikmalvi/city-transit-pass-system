using CTPS.API.Data;
using CTPS.API.DTOs.Pass;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        public UserRepository(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<User?> GetUserById(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user;
        }

        public async Task<List<UserPass>> GetUserPasses(int userId)
        {
            var passes = await context.UserPasses.
                Include(up => up.PassType).
                    ThenInclude(pt => pt.TransportModes)
                .Where(up => up.UserId == userId)
                .OrderByDescending(up => up.PurchaseDate)
                .ToListAsync();
            return passes;
        }
    }
}

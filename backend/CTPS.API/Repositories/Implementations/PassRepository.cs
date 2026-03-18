using CTPS.API.Data;
using CTPS.API.DTOs.Pass;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Repositories.Implementations
{
    public class PassRepository  : IPassRepository
    {
        private readonly AppDbContext context;

        public PassRepository(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<List<PassType>> GetAllPassTypes()
        {
            var result = await context.PassTypes
                .Include(p => p.TransportModes)
                .ToListAsync();
            return result;
        }
        public async Task<PassType?> GetPassTypeById(int passTypeId)
        {
            var passType = await context.PassTypes
               .Include(p => p.TransportModes)
               .FirstOrDefaultAsync(p => p.Id == passTypeId);
            return passType;
        }

        public async Task<int?> AddPass(UserPass request)
        {
            var createdUserPass = await context.UserPasses.AddAsync(request);
            await context.SaveChangesAsync();
            return createdUserPass.Entity.Id;
        }

        public async Task AutoExpirePasses(int userId)
        {
            var expiredPasses = await context.UserPasses
                .Where(up => up.UserId == userId
                    && up.Status == "Active"
                    && up.ExpiryDate < DateTime.Now)
                .ToListAsync();

            foreach (var pass in expiredPasses)
                pass.Status = "Expired";

            if (expiredPasses.Count > 0)
                await context.SaveChangesAsync();
        }

        public async Task<UserPass?> GetUserPassByCode(string passCode)
        {
            var userPass = await context.UserPasses
                .Include(up => up.User)
                .Include(up => up.PassType)
                    .ThenInclude(pt => pt!.TransportModes)
                .Include(up => up.Trips)
                .FirstOrDefaultAsync(up => up.PassCode == passCode);

            return userPass;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
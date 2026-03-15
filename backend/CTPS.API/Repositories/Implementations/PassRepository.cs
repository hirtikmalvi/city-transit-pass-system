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
    }
}

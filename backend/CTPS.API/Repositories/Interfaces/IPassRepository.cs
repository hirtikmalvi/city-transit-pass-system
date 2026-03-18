using CTPS.API.Common;
using CTPS.API.DTOs.Pass;
using CTPS.API.Models;

namespace CTPS.API.Repositories.Interfaces
{
    public interface IPassRepository
    {
        Task<List<PassType>> GetAllPassTypes();
        Task<PassType?> GetPassTypeById(int passTypeId);
        Task<int?> AddPass(UserPass request);
        Task AutoExpirePasses(int userId);
        Task<UserPass?> GetUserPassByCode(string passCode);
        Task SaveChangesAsync();
    }
}

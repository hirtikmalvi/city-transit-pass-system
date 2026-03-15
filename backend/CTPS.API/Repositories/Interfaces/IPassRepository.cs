using CTPS.API.Common;
using CTPS.API.DTOs.Pass;
using CTPS.API.Models;

namespace CTPS.API.Repositories.Interfaces
{
    public interface IPassRepository
    {
        Task<List<PassType>> GetAllPassTypes();
        Task<PassType?> GetPassTypeById(int passTypeId);
    }
}

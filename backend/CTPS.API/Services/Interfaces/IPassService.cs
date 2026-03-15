using CTPS.API.Common;
using CTPS.API.DTOs.Pass;

namespace CTPS.API.Services.Interfaces
{
    public interface IPassService
    {
        Task<Result<List<PassTypeResponseDTO>>> GetAllPassTypes();
    }
}

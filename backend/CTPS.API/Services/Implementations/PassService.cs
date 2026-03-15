using CTPS.API.Common;
using CTPS.API.Data;
using CTPS.API.DTOs.Pass;
using CTPS.API.Repositories.Interfaces;
using CTPS.API.Services.Interfaces;

namespace CTPS.API.Services.Implementations
{
    public class PassService: IPassService
    {
        private readonly IPassRepository passRepository;
        public PassService (IPassRepository _passRepository) 
        { 
            passRepository = _passRepository;
        }
        public async Task<Result<List<PassTypeResponseDTO>>> GetAllPassTypes()
        {
            var passTypes = await passRepository.GetAllPassTypes();

            var response = passTypes.Select(pt => new PassTypeResponseDTO
            {
                Id = pt.Id,
                Name = pt.Name,
                ValidityDays = pt.ValidityDays,
                Price = pt.Price,
                MaxTripsPerDay = pt.MaxTripsPerDay,
                TransportModes = pt.TransportModes.Select(tm => tm.Code).ToList()
            }).ToList();

            return Result<List<PassTypeResponseDTO>>.Ok(response, 200);
        }

    }
}

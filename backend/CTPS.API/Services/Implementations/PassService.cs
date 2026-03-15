using CTPS.API.Common;
using CTPS.API.Data;
using CTPS.API.DTOs.Pass;
using CTPS.API.Repositories.Interfaces;
using CTPS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Result<PassTypeResponseDTO?>> GetPassTypeById(int passTypeId)
        {
            var passType = await passRepository.GetPassTypeById(passTypeId);

            if (passType == null)
            {
                return Result<PassTypeResponseDTO?>.Fail(404);
            }

            var passTypeResponse = new PassTypeResponseDTO
            {
                Id = passType.Id,
                Name = passType.Name,
                ValidityDays = passType.ValidityDays,
                Price = passType.Price,
                MaxTripsPerDay = passType.MaxTripsPerDay,
                TransportModes = passType.TransportModes.Select(tm => tm.Code.ToString()).ToList()
            };
            return Result<PassTypeResponseDTO?>.Ok(passTypeResponse, 200);
        }
    }
}

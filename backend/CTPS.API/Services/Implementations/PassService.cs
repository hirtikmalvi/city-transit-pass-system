using CTPS.API.Common;
using CTPS.API.Data;
using CTPS.API.DTOs.Pass;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;
using CTPS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Services.Implementations
{
    public class PassService: IPassService
    {
        private readonly IPassRepository passRepository;
        private readonly IUserRepository userRepository;
        public PassService (IPassRepository _passRepository, IUserRepository _userRepository) 
        { 
            passRepository = _passRepository;
            userRepository = _userRepository;
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

        public async Task<Result<PurchasePassResponseDTO>> PurchasePass(PurchasePassRequestDTO request)
        {
            // Check user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
                return Result<PurchasePassResponseDTO>.Fail(404, ["User not found."]);

            // Check pass type exists
            var passType = await passRepository.GetPassTypeById(request.PassTypeId);

            if (passType == null)
                return Result<PurchasePassResponseDTO>.Fail(404, ["PassType not found."]);

            // Generate unique pass code
            var passCode = "PASS-" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

            var purchaseDate = DateTime.Now;
            var expiryDate = purchaseDate.AddDays(passType.ValidityDays);

            var userPass = new UserPass
            {
                UserId = request.UserId,
                PassTypeId = request.PassTypeId,
                PassCode = passCode,
                PurchaseDate = purchaseDate,
                ExpiryDate = expiryDate,
                Status = "Active"
            };

            // Add a Pass For User
            var createdPass = await passRepository.AddPass(userPass);

            var responseToReturn = Result<PurchasePassResponseDTO>.Ok(new PurchasePassResponseDTO
            {
                UserPassId = userPass.Id,
                PassCode = passCode,
                PassTypeName = passType.Name,
                PurchaseDate = purchaseDate,
                ExpiryDate = expiryDate,
                Status = "Active",
                CoveredTransportModes = passType.TransportModes.Select(tm => tm.Code).ToList()
            }, 201);

            return responseToReturn;
        }

        public async Task<Result<List<UserPassResponseDTO>>> GetUserPasses(int userId)
        {
            // Auto-expire passes before returning
            await passRepository.AutoExpirePasses(userId);

            var passes = await userRepository.GetUserPasses(userId);

            if (passes == null || passes.Count == 0)
            {
                return Result<List<UserPassResponseDTO>>.Fail(404);
            }

            var response = passes.Select(up => new UserPassResponseDTO
            {
                Id = up.Id,
                PassCode = up.PassCode,
                PassTypeName = up.PassType!.Name,
                Price = up.PassType.Price,
                PurchaseDate = up.PurchaseDate,
                ExpiryDate = up.ExpiryDate,
                Status = up.Status,
                CoveredTransportModes = up.PassType.TransportModes.Select(tm => tm.Code).ToList()
            }).ToList();

            return Result<List<UserPassResponseDTO>>.Ok(response, 200);
        }
    }
}

using CTPS.API.Common;
using CTPS.API.DTOs.Validation;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;
using CTPS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        private readonly ITripsRepository tripsRepository;
        private readonly IPassRepository passRepository;

        public ValidationService(ITripsRepository _tripsRepository, IPassRepository _passRepository)
        {
            tripsRepository = _tripsRepository;
            passRepository = _passRepository;
        }

        public async Task<Result<ValidatePassResponseDTO>> ValidatePass(ValidatePassRequestDTO request)
        {
            // Find pass by code — load everything needed in one query
            var userPass = await passRepository.GetUserPassByCode(request.PassCode);

            // if Pass not found
            if (userPass == null)
            {
                return Result<ValidatePassResponseDTO>.Fail(404);
            }
            // Rule 1: Check if pass is expired
            if (userPass.ExpiryDate < DateTime.UtcNow || userPass.Status == "Expired")
            {
                userPass.Status = "Expired";
                await passRepository.SaveChangesAsync();

                return Result<ValidatePassResponseDTO>.Ok(new ValidatePassResponseDTO
                {
                    IsValid = false,
                    Message = "Pass expired.",
                    PassHolderName = userPass.User?.Name,
                    PassTypeName = userPass.PassType?.Name,
                    ExpiryDate = userPass.ExpiryDate
                }, 200);
            }

            // RULE 2: Transport mode
            var coveredModes = userPass.PassType!.TransportModes.Select(tm => tm.Code.ToUpper()).ToList();

            if (!coveredModes.Contains(request.TransportModeCode.ToUpper()))
                return Result<ValidatePassResponseDTO>.Ok(new ValidatePassResponseDTO
                {
                    IsValid = false,
                    Message = $"Transport mode not covered. This pass is valid for: {string.Join(", ", coveredModes)}",
                    PassHolderName = userPass.User?.Name,
                    PassTypeName = userPass.PassType.Name,
                    ExpiryDate = userPass.ExpiryDate
                }, 200);

            // RULE 3: Check Daily limit
                if (userPass.PassType.MaxTripsPerDay.HasValue)
                {
                    var tripsToday = userPass.Trips.Count(t => t.ValidatedAt.HasValue && t.ValidatedAt.Value.Date == DateTime.Now);
                    if (tripsToday >= userPass.PassType.MaxTripsPerDay.Value)
                        return Result<ValidatePassResponseDTO>.Ok(new ValidatePassResponseDTO
                        {
                            IsValid = false,
                            Message = $"Daily trip limit reached. Maximum {userPass.PassType.MaxTripsPerDay} trips per day allowed.",
                            PassHolderName = userPass.User?.Name,
                            PassTypeName = userPass.PassType.Name,
                            ExpiryDate = userPass.ExpiryDate,
                            TripsUsedToday = tripsToday,
                            MaxTripsPerDay = userPass.PassType.MaxTripsPerDay
                        }, 200);
                }

            // RULE 4: Anti-passback
            var lastTrip = userPass.Trips.Where(t => t.ValidatedAt.HasValue).OrderByDescending(t => t.ValidatedAt).FirstOrDefault();
            if (lastTrip != null)
            {
                var minutesSinceLast = (DateTime.Now - lastTrip.ValidatedAt!.Value).TotalMinutes;
                if (minutesSinceLast < 5)
                {
                    var waitSeconds = (int)((5 * 60) - (DateTime.Now - lastTrip.ValidatedAt.Value).TotalSeconds);

                    return Result<ValidatePassResponseDTO>.Ok(new ValidatePassResponseDTO
                    {
                        IsValid = false,
                        Message = $"Please wait before next validation. Try again in {waitSeconds} seconds.",
                        PassHolderName = userPass.User?.Name,
                        PassTypeName = userPass.PassType.Name,
                        ExpiryDate = userPass.ExpiryDate
                    }, 200);
                }
            }

            // if ALL ruLe PASSED then record trip
            var trip = new Trip { 
                UserPassId = userPass.Id, 
                ValidatedBy = request.ValidatedByUserId, 
                TransportMode = request.TransportModeCode.ToUpper(), 
                RouteInfo = request.RouteInfo, 
                ValidatedAt = DateTime.Now
            };

            await tripsRepository.AddTrip(trip);

            var todayCount = userPass.Trips.Count(t => t.ValidatedAt.HasValue && t.ValidatedAt.Value.Date == DateTime.Today) + 1;

            return Result<ValidatePassResponseDTO>.Ok(new ValidatePassResponseDTO
            {
                IsValid = true,
                Message = "Pass validated successfully. Have a safe journey!",
                PassHolderName = userPass.User?.Name,
                PassTypeName = userPass.PassType.Name,
                ExpiryDate = userPass.ExpiryDate,
                TripsUsedToday = todayCount,
                MaxTripsPerDay = userPass.PassType.MaxTripsPerDay
            }, 200);
        
        }
    }
}

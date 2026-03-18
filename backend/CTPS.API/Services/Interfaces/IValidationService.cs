using CTPS.API.Common;
using CTPS.API.DTOs.Validation;

namespace CTPS.API.Services.Interfaces
{
    public interface IValidationService
    {
        Task<Result<ValidatePassResponseDTO>> ValidatePass(ValidatePassRequestDTO request);
    }
}

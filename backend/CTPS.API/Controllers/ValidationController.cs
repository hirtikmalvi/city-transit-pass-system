using CTPS.API.Common;
using CTPS.API.DTOs.Validation;
using CTPS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTPS.API.Controllers
{
    [Route("api/validation")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationService validationService;

        public ValidationController(IValidationService _validationService)
        {
            validationService = _validationService;
        }

        [HttpPost("validate")]
        public async Task<ActionResult<Result<ValidatePassResponseDTO>>> Validate(
        [FromBody] ValidatePassRequestDTO request)
        {
            var result = await validationService.ValidatePass(request);
            return Ok(result);
        }
    }
}

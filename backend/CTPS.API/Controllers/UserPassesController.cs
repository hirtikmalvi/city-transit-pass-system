using CTPS.API.Common;
using CTPS.API.DTOs.Pass;
using CTPS.API.Services.Implementations;
using CTPS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTPS.API.Controllers
{
    [Route("api/userpasses")]
    [ApiController]
    public class UserPassesController : ControllerBase
    {
        private readonly IPassService passService;
        public UserPassesController(IPassService _passService)
        {
            passService = _passService;
        }

        // POST /api/userpasses/purchase
        [HttpPost("purchase")]
        public async Task<ActionResult<Result<PurchasePassResponseDTO>>> Purchase(
            [FromBody] PurchasePassRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)).ToList();
                return Ok(Result<PurchasePassResponseDTO>.Fail(400, errors));
            }

            var result = await passService.PurchasePass(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET /api/userpasses/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserPassResponseDTO>>> GetUserPasses([FromRoute] int userId)
        {
            var result = await passService.GetUserPasses(userId);
            return Ok(result);
        }
    }
}

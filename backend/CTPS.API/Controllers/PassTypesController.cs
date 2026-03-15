using CTPS.API.Common;
using CTPS.API.DTOs.Pass;
using CTPS.API.Services.Implementations;
using CTPS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTPS.API.Controllers
{
    [Route("api/passes")]
    [ApiController]
    public class PassTypesController : ControllerBase
    {
        private readonly IPassService passService;
        public PassTypesController(IPassService _passService) 
        { 
            passService = _passService;
        }

        // GET /api/passes/types
        [HttpGet("types")]
        public async Task<ActionResult<Result<List<PassTypeResponseDTO>>>> GetAllPassTypes()
        {
            var result = await passService.GetAllPassTypes();
            return Ok(result);
        }

        // GET /api/passes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<PassTypeResponseDTO>>> GetPassById([FromRoute] int id) 
        {
            var result = await passService.GetPassTypeById(id);
            return Ok(result);
        }
    }
}

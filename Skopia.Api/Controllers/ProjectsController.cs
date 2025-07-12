using Microsoft.AspNetCore.Mvc;
using Skopia.Domain.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Api.Controllers
{
    [ApiController]
    [Route("api/v1.0/skopia/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long> _service;

        public ProjectsController(IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long> service) => _service = service;

        [HttpPost("new")]
        public async Task<ActionResult<ProjectResponseDTO>> Create(ProjectRequestDTO request)
        {
            var result = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProjectResponseDTO>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<ProjectResponseDTO>> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return NoContent();
        }
    }
}
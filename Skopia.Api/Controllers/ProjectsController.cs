using Microsoft.AspNetCore.Mvc;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Api.Controllers
{
    [ApiController]
    [Route("api/v1.0/skopia/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service) => _service = service;

        [HttpPost("post")]
        public async Task<ActionResult<ProjectResponseDTO>> Post(ProjectRequestDTO request)
        {
            var result = await _service.PostAsync(request);

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
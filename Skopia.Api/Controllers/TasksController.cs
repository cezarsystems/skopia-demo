using Microsoft.AspNetCore.Mvc;
using Skopia.Domain.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Api.Controllers
{
    [ApiController]
    [Route("api/v1.0/skopia/projects/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long> _service;

        public TasksController(IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long> service) => _service = service;

        [HttpPost("new")]
        public async Task<ActionResult<TaskResponseDTO>> Create(TaskRequestDTO request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<TaskResponseDTO>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<TaskResponseDTO>> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<TaskResponseDTO>> Update(long id, [FromBody] TaskUpdateRequestDTO request)
        {
            var result = await _service.UpdateAsync(id, request);

            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            if (result.Data == null)
                return NotFound();

            return Ok(result.Data);
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
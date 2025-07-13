using Microsoft.AspNetCore.Mvc;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Api.Controllers
{
    [ApiController]
    [Route("api/v1.0/skopia/projects/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service) => _service = service;

        [HttpPost("post")]
        public async Task<ActionResult<TaskResponseDTO>> Post(TaskRequestDTO request)
        {
            var result = await _service.PostAsync(request);
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

        [HttpPut("update")]
        public async Task<ActionResult<TaskResponseDTO>> Update(TaskUpdateRequestDTO request)
        {
            var result = await _service.UpdateAsync(request);

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
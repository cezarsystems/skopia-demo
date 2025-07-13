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
        private readonly IGetOperations<TaskResponseDTO, long> _get;
        private readonly IPostOperations<TaskRequestDTO, TaskResponseDTO> _post;
        private readonly IUpdateOperations<TaskUpdateRequestDTO, TaskResponseDTO> _update;
        private readonly IDeleteOperations<long> _delete;


        public TasksController(
            IGetOperations<TaskResponseDTO, long> get,
            IPostOperations<TaskRequestDTO, TaskResponseDTO> post,
            IUpdateOperations<TaskUpdateRequestDTO, TaskResponseDTO> update,
            IDeleteOperations<long> delete)
        {
            _get = get;
            _post = post;
            _update = update;
            _delete = delete;
        }

        [HttpPost("post")]
        public async Task<ActionResult<TaskResponseDTO>> Post(TaskRequestDTO request)
        {
            var result = await _post.PostAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<TaskResponseDTO>> GetById(long id)
        {
            var result = await _get.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<TaskResponseDTO>> GetAll()
        {
            var result = await _get.GetAllAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<TaskResponseDTO>> Update([FromBody] TaskUpdateRequestDTO request)
        {
            var result = await _update.UpdateAsync(request);

            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            if (result.Data == null)
                return NotFound();

            return Ok(result.Data);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _delete.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return NoContent();
        }
    }
}
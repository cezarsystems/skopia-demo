using Microsoft.AspNetCore.Mvc;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Api.Controllers
{
    /// <summary>
    /// Responsável pela gestão de tarefas associadas a projetos.
    /// </summary>
    [ApiController]
    [Route("api/v1.0/skopia/projects/tasks")]
    [Tags("Tarefas")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service) => _service = service;

        /// <summary>
        /// Cria uma nova tarefa vinculada a um projeto.
        /// </summary>
        /// <remarks>
        /// A tarefa será associada ao usuário e projeto informados na requisição.
        /// </remarks>
        /// <param name="request">Dados da tarefa a ser criada.</param>
        /// <returns>Dados da tarefa criada.</returns>
        [HttpPost("post")]
        public async Task<ActionResult<TaskResponseDTO>> Post(TaskRequestDTO request)
        {
            var result = await _service.PostAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Obtém os detalhes de uma tarefa específica.
        /// </summary>
        /// <param name="id">Identificador da tarefa.</param>
        /// <returns>Informações completas da tarefa.</returns>
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TaskResponseDTO>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Lista todas as tarefas existentes no sistema.
        /// </summary>
        /// <returns>Lista de todas as tarefas cadastradas.</returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<TaskResponseDTO>> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de uma tarefa existente.
        /// </summary>
        /// <remarks>
        /// Permite modificar o status, data de expiração ou adicionar comentários. Também registra histórico das alterações.
        /// </remarks>
        /// <param name="request">Dados atualizados da tarefa.</param>
        /// <returns>Dados da tarefa após a atualização.</returns>
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

        /// <summary>
        /// Exclui uma tarefa existente.
        /// </summary>
        /// <param name="id">Identificador da tarefa a ser removida.</param>
        /// <returns>Resultado da operação.</returns>
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
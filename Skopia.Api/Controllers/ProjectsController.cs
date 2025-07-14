using Microsoft.AspNetCore.Mvc;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Api.Controllers
{
    /// <summary>
    /// Responsável pelo gerenciamento de projetos no sistema.
    /// </summary>
    [ApiController]
    [Route("api/v1.0/skopia/projects")]
    [Tags("Projetos")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service) => _service = service;

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        /// <remarks>
        /// Cria e persiste um novo projeto com base nas informações fornecidas.
        /// </remarks>
        /// <param name="request">Objeto contendo os dados do projeto a ser criado.</param>
        /// <returns>Dados do projeto recém-criado.</returns>
        [HttpPost("post")]
        public async Task<ActionResult<ProjectResponseDTO>> Post(ProjectRequestDTO request)
        {
            var result = await _service.PostAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Obtém os detalhes de um projeto específico.
        /// </summary>
        /// <param name="id">Identificador do projeto.</param>
        /// <returns>Informações detalhadas do projeto correspondente.</returns>
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProjectResponseDTO>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os projetos existentes.
        /// </summary>
        /// <returns>Conjunto de todos os projetos registrados no sistema.</returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<ProjectResponseDTO>> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        /// <summary>
        /// Exclui um projeto pelo seu identificador.
        /// </summary>
        /// <remarks>
        /// Permite a exclusão apenas de projetos cujas tarefas associadas não estejam com o status "Pendente".
        /// </remarks>
        /// <param name="id">Identificador do projeto a ser removido.</param>
        /// <returns>Resultado da operação de exclusão.</returns>
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
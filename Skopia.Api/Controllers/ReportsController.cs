using Microsoft.AspNetCore.Mvc;
using Skopia.Api.Middleware.Filters;
using Skopia.Application.Contracts;

namespace Skopia.Api.Controllers
{
    /// <summary>
    /// Fornece relatórios analíticos sobre projetos e tarefas.
    /// Acesso restrito a usuários com perfil de gerente.
    /// </summary>
    [OnlyManager]
    [ApiController]
    [Route("api/v1.0/skopia/reports")]
    [Tags("Relatórios")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService) => _reportsService = reportsService;

        /// <summary>
        /// Retorna o desempenho dos usuários com base nas tarefas concluídas nos últimos 30 dias.
        /// </summary>
        /// <remarks>
        /// Este relatório agrupa os usuários e contabiliza quantas tarefas cada um concluiu no período.
        /// </remarks>
        /// <param name="userId">Identificador do usuário solicitante (usado para controle de acesso).</param>
        [HttpGet("user-task-performance")]
        public async Task<IActionResult> GetUserPerformance([FromQuery] long userId) =>
            Ok(await _reportsService.GetUserPerformanceAsync());

        /// <summary>
        /// Retorna estatísticas de projetos criados e tarefas associadas nos últimos 30 dias.
        /// </summary>
        /// <remarks>
        /// Exibe também a média de tarefas por projeto no período analisado.
        /// </remarks>
        /// <param name="userId">Identificador do usuário solicitante.</param>
        [HttpGet("project-stats")]
        public async Task<IActionResult> GetProjectStats([FromQuery] long userId) =>
            Ok(await _reportsService.GetProjectStatsAsync());

        /// <summary>
        /// Retorna o tempo médio de conclusão das tarefas.
        /// </summary>
        /// <remarks>
        /// O relatório pode ser filtrado por projeto e por período de data.
        /// O tempo é apresentado em horas e dias.
        /// </remarks>
        /// <param name="projectId">Identificador do projeto (opcional).</param>
        /// <param name="startDate">Data inicial do filtro (opcional).</param>
        /// <param name="endDate">Data final do filtro (opcional).</param>
        /// <param name="userId">Identificador do usuário solicitante.</param>
        [HttpGet("task-completion-time")]
        public IActionResult GetAvgCompletionTime(
            [FromQuery] long? projectId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] long userId) =>
            Ok(_reportsService.GetAverageTaskCompletionTime(projectId, startDate, endDate));

        /// <summary>
        /// Retorna uma visão geral das tarefas agrupadas por projeto.
        /// </summary>
        /// <remarks>
        /// Permite aplicar filtros por status, prioridade e período de data.  
        /// Exibe para cada projeto a lista de tarefas com nome, status, prioridade e datas.
        /// </remarks>
        /// <param name="status">Status da tarefa (opcional).</param>
        /// <param name="priority">Prioridade da tarefa (opcional).</param>
        /// <param name="startDate">Data inicial para o filtro (opcional).</param>
        /// <param name="endDate">Data final para o filtro (opcional).</param>
        /// <param name="userId">Identificador do usuário solicitante.</param>
        [HttpGet("task-overview")]
        public async Task<IActionResult> GetTasksPerProject(
            [FromQuery] string? status,
            [FromQuery] string? priority,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] long userId) =>
            Ok(await _reportsService.GetProjectTasksReportAsync(status, priority, startDate, endDate));
    }
}
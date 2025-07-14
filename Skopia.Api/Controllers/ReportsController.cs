using Microsoft.AspNetCore.Mvc;
using Skopia.Api.Middleware.Filters;
using Skopia.Application.Contracts;

namespace Skopia.Api.Controllers
{
    [OnlyManager]
    [ApiController]
    [Route("api/v1.0/skopia/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService) => _reportsService = reportsService;

        [HttpGet("user-task-performance")]
        public async Task<IActionResult> GetUserPerformance([FromQuery] long userId) => Ok(await _reportsService.GetUserPerformanceAsync());

        [HttpGet("project-stats")]
        public async Task<IActionResult> GetProjectStats([FromQuery] long userId) => Ok(await _reportsService.GetProjectStatsAsync());

        [HttpGet("task-completion-time")]
        public IActionResult GetAvgCompletionTime(
            [FromQuery] long? projectId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] long userId) => Ok(_reportsService.GetAverageTaskCompletionTime(projectId, startDate, endDate));

        [HttpGet("task-overview")]
        public async Task<IActionResult> GetTasksPerProject(
            [FromQuery] string? status,
            [FromQuery] string? priority,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] long userId) => Ok(await _reportsService.GetProjectTasksReportAsync(status, priority, startDate, endDate));
    }
}
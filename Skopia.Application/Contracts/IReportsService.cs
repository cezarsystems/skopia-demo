using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts
{
    public interface IReportsService
    {
        Task<IEnumerable<UserPerformanceResponseDTO>> GetUserPerformanceAsync();
        Task<ProjectStatsResponseDTO> GetProjectStatsAsync();
        IEnumerable<TaskCompletionTimeResponseDTO> GetAverageTaskCompletionTime(
            long? projectId = null,
            DateTime? startDate = null,
            DateTime? endDate = null);

        Task<IEnumerable<ProjectTasksReportResponseDTO>> GetProjectTasksReportAsync(
            string? status = null,
            string? priority = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}
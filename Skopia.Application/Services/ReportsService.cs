using Microsoft.EntityFrameworkCore;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly SkopiaDbContext _dbContext;

        public ReportsService(SkopiaDbContext dbContext) => _dbContext = dbContext;

        public async Task<ProjectStatsResponseDTO> GetProjectStatsAsync()
        {
            var recentProjects = await _dbContext.Projects
                .AsNoTracking()
                .Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                .Include(p => p.Tasks)
                .ToListAsync();

            int totalProjects = recentProjects.Count;
            int totalTasks = recentProjects.Sum(p => p.Tasks?.Count ?? 0);
            double average = totalProjects > 0 ? (double)totalTasks / totalProjects : 0;

            return new ProjectStatsResponseDTO
            {
                TotalProjectsLast30Days = totalProjects,
                TotalTasksLast30Days = totalTasks,
                AverageTasksPerProject = Math.Round(average, 2)
            };
        }

        public async Task<IEnumerable<UserPerformanceResponseDTO>> GetUserPerformanceAsync()
        {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.CompletedAt >= DateTime.UtcNow.AddDays(-30) && t.Status == StatusEnum.C.ToString())
                .GroupBy(t => t.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalCompleted = g.Count()
                })
                .Join(_dbContext.Users
                .AsNoTracking(),
                    g => g.UserId,
                    u => u.Id,
                    (g, u) => new UserPerformanceResponseDTO
                    {
                        UserId = u.Id,
                        UserName = u.Name,
                        Role = u.Role,
                        TotalCompletedTasks = g.TotalCompleted
                    })
                .ToListAsync();
        }

        public IEnumerable<TaskCompletionTimeResponseDTO> GetAverageTaskCompletionTime(
            long? projectId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.Project)
                .Where(t =>
                    t.Status == StatusEnum.C.ToString() &&
                    t.CreatedAt != null &&
                    t.CompletedAt != null);

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CompletedAt <= endDate.Value);

            return query
                .AsEnumerable()
                .GroupBy(t => new { t.ProjectId, t.Project.Name })
                .Select(g => new TaskCompletionTimeResponseDTO
                {
                    ProjectId = g.Key.ProjectId,
                    ProjectName = g.Key.Name,
                    TaskCount = g.Count(),
                    AverageCompletionTimeInHours = Math.Round(g.Average(t => (t.CompletedAt!.Value - t.CreatedAt).TotalHours), 2),
                    AverageCompletionTimeInDays = Math.Round(g.Average(t => (t.CompletedAt!.Value - t.CreatedAt).TotalDays), 2)
                })
                .ToList();
        }

        public async Task<IEnumerable<ProjectTasksReportResponseDTO>> GetProjectTasksReportAsync(
            string? status = null,
            string? priority = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.Project)
                .Where(t => t.Project != null);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status == status.ToUpper());

            if (!string.IsNullOrWhiteSpace(priority))
                query = query.Where(t => t.Priority == priority.ToUpper());

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedAt <= endDate.Value);

            var tasks = await query
                .Select(t => new
                {
                    t.ProjectId,
                    ProjectName = t.Project.Name,
                    TaskName = t.Name,
                    t.CreatedAt,
                    t.Status,
                    t.Priority
                })
                .ToListAsync();

            return tasks
                .GroupBy(t => new { t.ProjectId, t.ProjectName })
                .Select(g => new ProjectTasksReportResponseDTO
                {
                    ProjectId = g.Key.ProjectId,
                    ProjectName = g.Key.ProjectName,
                    Tasks = g.Select(t => new SimpleTaskInfoResponseDTO
                    {
                        TaskName = t.TaskName,
                        CreatedAt = t.CreatedAt,
                        Status = ToolsServiceExtension.GetEnumDescription<StatusEnum>(t.Status),
                        Priority = ToolsServiceExtension.GetEnumDescription<PriorityEnum>(t.Priority)
                    })
                })
                .ToList();
        }
    }
}
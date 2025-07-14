namespace Skopia.DTOs.Models.Response
{
    public class ProjectStatsResponseDTO
    {
        public int TotalProjectsLast30Days { get; set; }
        public int TotalTasksLast30Days { get; set; }
        public double AverageTasksPerProject { get; set; }
    }
}
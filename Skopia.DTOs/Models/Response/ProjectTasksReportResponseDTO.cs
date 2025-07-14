namespace Skopia.DTOs.Models.Response
{
    public class ProjectTasksReportResponseDTO
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<SimpleTaskInfoResponseDTO> Tasks { get; set; }
    }
}
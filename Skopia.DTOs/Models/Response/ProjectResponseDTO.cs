namespace Skopia.DTOs.Models.Response
{
    public class ProjectResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<TaskResponseDTO> Tasks { get; set; }
    }
}
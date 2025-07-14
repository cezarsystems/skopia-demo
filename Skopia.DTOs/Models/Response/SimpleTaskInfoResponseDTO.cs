namespace Skopia.DTOs.Models.Response
{
    public class SimpleTaskInfoResponseDTO
    {
        public string TaskName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
    }
}
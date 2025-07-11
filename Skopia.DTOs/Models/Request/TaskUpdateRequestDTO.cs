namespace Skopia.DTOs.Models.Request
{
    public class TaskUpdateRequestDTO
    {
        public long TaskId { get; set; }
        public char Status { get; set; }
        public DateTime? ExpirationData { get; set; }
        public string Comment { get; set; }
    }
}
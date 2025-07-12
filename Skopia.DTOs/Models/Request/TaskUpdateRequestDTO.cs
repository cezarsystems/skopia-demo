namespace Skopia.DTOs.Models.Request
{
    public class TaskUpdateRequestDTO
    {
        public long TaskId { get; set; }
        public long UserId { get; set; }
        public char Status { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Comment { get; set; }
    }
}
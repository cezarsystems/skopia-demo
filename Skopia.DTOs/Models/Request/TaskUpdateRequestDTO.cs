namespace Skopia.DTOs.Models.Request
{
    public class TaskUpdateRequestDTO
    {
        public long TaskId { get; set; }
        public long UserId { get; set; }
        public string Status { get; set; }
        public string ExpirationDate { get; set; }
        public string Comment { get; set; }
    }
}
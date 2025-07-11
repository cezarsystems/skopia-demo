namespace Skopia.DTOs.Models.Request
{
    public class TaskRequestDTO
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public char Priority { get; set; }
        public char Status { get; set; }
        public DateTime? ExpirationData { get; set; }
        public string Comment { get; set; }
    }
}
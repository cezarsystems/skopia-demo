namespace Skopia.DTOs.Models.Request
{
    public class TaskRequestDTO
    {
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string ExpirationDate { get; set; }
        public string Comment { get; set; }
    }
}
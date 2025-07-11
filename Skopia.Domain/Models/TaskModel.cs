namespace Skopia.Domain.Models
{
    public class TaskModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime? ExpirationData { get; set; }
        public string[] Comments { get; set; }
        public long ProjectId { get; set; }
        public ProjectModel Project { get; set; }
    }
}
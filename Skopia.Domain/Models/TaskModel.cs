namespace Skopia.Domain.Models
{
    public class TaskModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public long ProjectId { get; set; }
        public ProjectModel Project { get; set; }

        public long UserId { get; set; }
        public UserModel User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public ICollection<TaskHistoryModel> Histories { get; set; } = new List<TaskHistoryModel>();
        public ICollection<TaskCommentModel> Comments { get; set; } = new List<TaskCommentModel>();
    }
}
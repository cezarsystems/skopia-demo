namespace Skopia.Domain.Models
{
    public class TaskCommentModel
    {
        public long Id { get; set; }

        public long TaskId { get; set; }
        public TaskModel Task { get; set; }

        public long UserId { get; set; }
        public UserModel User { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
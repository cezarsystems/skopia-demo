namespace Skopia.Domain.Models
{
    public class TaskHistoryModel
    {
        public long Id { get; set; }

        public long TaskId { get; set; }
        public TaskModel Task { get; set; }

        public long UserId { get; set; }
        public UserModel User { get; set; }

        public string FieldChanged { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
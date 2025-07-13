namespace Skopia.Domain.Models
{
    public class ProjectModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public long UserId { get; set; }
        public UserModel User { get; set; }

        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
namespace Skopia.Domain.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public ICollection<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public ICollection<TaskHistoryModel> Histories { get; set; } = new List<TaskHistoryModel>();
        public ICollection<TaskCommentModel> Comments { get; set; } = new List<TaskCommentModel>();
    }
}
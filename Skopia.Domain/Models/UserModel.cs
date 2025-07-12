namespace Skopia.Domain.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public ICollection<ProjectModel> Projects { get; set; }
        public ICollection<TaskModel> Tasks { get; set; }
        public ICollection<TaskHistoryModel> Histories { get; set; }
    }
}
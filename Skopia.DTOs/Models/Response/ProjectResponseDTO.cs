namespace Skopia.DTOs.Models.Response
{
    public class ProjectResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short NumberOfTasks { get; set; }
        public DateTime LastModified { get; set; }
        public UserInfoResponseDTO Creator { get; set; }
    }
}
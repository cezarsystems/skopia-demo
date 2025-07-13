namespace Skopia.DTOs.Models.Response
{
    public class TaskResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime LastModified { get; set; }
        public string[] Comments { get; set; }
        public UserInfoResponseDTO Creator { get; set; }
        public ProjectBasicInfoResponseDTO Project { get; set; }
    }
}
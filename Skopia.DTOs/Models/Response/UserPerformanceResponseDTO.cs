namespace Skopia.DTOs.Models.Response
{
    public class UserPerformanceResponseDTO
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public int TotalCompletedTasks { get; set; }
    }
}
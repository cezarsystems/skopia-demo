namespace Skopia.DTOs.Models.Response
{
    public class TaskCompletionTimeResponseDTO
    {
        public long? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int TaskCount { get; set; }
        public double AverageCompletionTimeInHours { get; set; }
        public double AverageCompletionTimeInDays { get; set; }
    }
}
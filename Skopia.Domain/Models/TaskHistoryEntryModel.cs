namespace Skopia.Domain.Models
{
    public class TaskHistoryEntryModel
    {
        public long TaskId { get; set; }
        public long UserId { get; set; }
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
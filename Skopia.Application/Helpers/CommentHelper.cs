namespace Skopia.Application.Helpers
{
    public static class CommentHelper
    {
        public static string AddComment(string comment, string userName)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm}] {userName}: {comment ?? string.Empty}";
        }

        public static string AppendComment(string existingComments, string newComment, string userName)
        {
            if (string.IsNullOrWhiteSpace(newComment))
                return existingComments ?? string.Empty;

            var formatted = AddComment(newComment, userName);

            if (string.IsNullOrWhiteSpace(existingComments))
                return formatted;

            return $"{existingComments}{Environment.NewLine}{formatted}";
        }
    }
}
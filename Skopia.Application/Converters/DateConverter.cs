namespace Skopia.Application.Converters
{
    public static class DateConverter
    {
        public static DateTime? Parse(string input) => DateTime.TryParse(input, out var result) ? result : null;
    }
}
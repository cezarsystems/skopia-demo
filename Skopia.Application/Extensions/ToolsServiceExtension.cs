using System.ComponentModel;
using System.Reflection;

public static class ToolsServiceExtension
{
    public static string GetEnumDescription<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return "N/D";

        var match = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .FirstOrDefault(e => e
            .ToString()
            .Equals(value, StringComparison.OrdinalIgnoreCase));

        var memberInfo = typeof(TEnum)
            .GetMember(match
            .ToString());

        if (memberInfo.Length > 0)
        {
            var attribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();

            if (attribute != null)
                return attribute.Description;
        }

        return match.ToString();
    }
}
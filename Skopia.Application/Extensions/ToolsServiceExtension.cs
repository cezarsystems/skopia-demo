using System.ComponentModel;
using System.Reflection;

public static class ToolsServiceExtension
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        if (enumValue == null)
            return "N/D";

        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());
        if (memberInfo.Length > 0)
        {
            var attribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null)
                return attribute.Description;
        }

        return enumValue.ToString();
    }
}
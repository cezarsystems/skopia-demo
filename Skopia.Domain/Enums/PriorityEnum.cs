using System.ComponentModel;

namespace Skopia.Domain.Enums
{
    public enum PriorityEnum
    {
        [Description("Baixa")]
        Low,
        [Description("Média")]
        Medium,
        [Description("Alta")]
        High
    }
}
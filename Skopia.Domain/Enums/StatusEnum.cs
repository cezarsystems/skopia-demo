using System.ComponentModel;

namespace Skopia.Domain.Enums
{
    public enum StatusEnum
    {
        [Description("Pendente")]
        P,
        [Description("Em andamento")]
        A,
        [Description("Concluída")]
        C
    }
}
using System.ComponentModel;

namespace Skopia.Domain.Enums
{
    public enum StatusEnum
    {
        [Description("Pendente")]
        Waiting,
        [Description("Em andamento")]
        Working,
        [Description("Concluída")]
        Done
    }
}
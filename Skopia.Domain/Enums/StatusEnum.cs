using System.ComponentModel;

public enum StatusEnum
{
    /// <summary>
    /// Status: Pendente - Indica que a tarefa está pendente (ainda não iniciada)
    /// </summary>
    [Description("Pendente")] P,
    /// <summary>
    /// Status: Em andamento - Indica que a tarefa ainda está em andamento (não sendo possível excluir o projeto)
    /// </summary>
    [Description("Em andamento")] A,
    /// <summary>
    /// Status: Concluída - Indica que a tarefa já foi concluída
    /// </summary>
    [Description("Concluída")] C
}
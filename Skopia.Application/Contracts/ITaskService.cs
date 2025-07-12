using Skopia.Domain.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts
{
    public interface ITaskService : IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long>
    {
        // TODO: add validações de tarefas
    }
}
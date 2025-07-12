using Skopia.Domain.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts
{
    public interface IProjectService : IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long>
    {
        Task<bool> Exists(long id);
    }
}
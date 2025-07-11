using Skopia.Domain.Contracts;
using Skopia.Domain.Enums;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Services
{
    public class ProjectService : IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long>
    {
        public Task<ProjectResponseDTO> CreateAsync(ProjectRequestDTO request)
        {
            return Task.Run(() => new ProjectResponseDTO
            {
                Id = new Random().Next(1, 100000),
                Name = request.Name,
                Description = "Minha decrição de projeto!",
                Tasks =
                [
                    new()
                    {
                        Name = "Tarefa pesada",
                        Description = "Uma tarefa de teste",
                        Comments =
                        [
                            "Comentário 1", "Comentário 2"
                        ],
                        Status = StatusEnum.Done.GetEnumDescription()
                    }
                ]
            });
        }

        public Task<OperationResultModel> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectResponseDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectResponseDTO> GetByIdAsync(long id)
        {
            return Task.Run(() => new ProjectResponseDTO
            {
                Id = 69,
                Name = "Projeto Piloto 0",
                Tasks =
                [
                    new()
                    {
                        Name = "Tarefa 1",
                        Description = "Uma tarefa de teste",
                        Comments =
                        [
                            "Comentário 1", "Comentário 2"
                        ],
                        Status = StatusEnum.Done.GetEnumDescription()
                    }
                ]
            });
        }

        // Não há RN definida para atualização (update) de projetos
        public Task<OperationResultModel<ProjectResponseDTO>> UpdateAsync(long id, ProjectRequestDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
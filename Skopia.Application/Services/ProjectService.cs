using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skopia.Application.Contracts;
using Skopia.Domain.Contracts;
using Skopia.Domain.Enums;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class ProjectService : IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long>, IProjectService
    {
        private readonly SkopiaDbContext _dbContext;

        private readonly IMapper _mapper;

        public ProjectService(SkopiaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDTO> CreateAsync(ProjectRequestDTO request)
        {
            var newProject = _mapper.Map<ProjectModel>(request);
            newProject.LastModified = DateTime.Now;
            newProject.Tasks = [];

            _dbContext.Projects.Add(newProject);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectResponseDTO>(newProject);
        }

        public async Task<OperationResultModel> DeleteAsync(long id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return OperationResultModel.Fail("Projeto não encontrado. Favor confirmar seu identificador.");
            }

            var hasPendingTasks = project.Tasks.Any(t => t.Status == StatusEnum.P.ToString());

            if (hasPendingTasks)
            {
                return OperationResultModel.Fail("Não é possível excluir o projeto, pois existem tarefas com status: Pendente");
            }

            _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync();

            return OperationResultModel.Ok();
        }


        public async Task<IEnumerable<ProjectResponseDTO>> GetAllAsync()
        {
            var projects = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.Tasks)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectResponseDTO>>(projects);
        }

        public async Task<ProjectResponseDTO> GetByIdAsync(long id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            return _mapper.Map<ProjectResponseDTO>(project);
        }

        public async Task<bool> Exists(long id)
        {
            return await _dbContext.Projects.AnyAsync(p => p.Id == id);
        }

        // Não há RN definida para atualização (update) de projetos
        public Task<OperationResultModel<ProjectResponseDTO>> UpdateAsync(long id, ProjectRequestDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skopia.Application.Contracts;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly SkopiaDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProjectService(SkopiaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDTO> PostAsync(ProjectRequestDTO request)
        {
            var newProject = _mapper.Map<ProjectModel>(request);
            newProject.CreatedAt = DateTime.Now;
            newProject.Tasks = [];

            _dbContext.Projects.Add(newProject);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newProject)
                .Reference(p => p.User)
                .LoadAsync();

            return _mapper.Map<ProjectResponseDTO>(newProject);
        }

        public async Task<OperationResponseDTO> DeleteAsync(long id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return OperationResponseDTO.Fail("Projeto não encontrado. Favor confirmar seu identificador.");
            }

            var hasPendingTasks = project.Tasks?.Any(t => t.Status == StatusEnum.P.ToString()) == true;

            if (hasPendingTasks)
            {
                return OperationResponseDTO.Fail("Não é possível excluir o projeto, pois existem tarefas com status: Pendente. Favor concluí-las ou removê-las antes da exclusão");
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return OperationResponseDTO.Ok();
        }

        public async Task<IEnumerable<ProjectResponseDTO>> GetAllAsync()
        {
            var projects = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectResponseDTO>>(projects);
        }

        public async Task<ProjectResponseDTO> GetByIdAsync(long id)
        {
            var project = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            return _mapper.Map<ProjectResponseDTO>(project);
        }

        public async Task<bool> Exists(long id) => await _dbContext.Projects.AnyAsync(p => p.Id == id);
    }
}
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts;

public interface IProjectService :
    ICheckExistence<long>,
    IDeleteOperations<long>,
    IGetOperations<ProjectResponseDTO, long>,
    IPostOperations<ProjectRequestDTO, ProjectResponseDTO>;
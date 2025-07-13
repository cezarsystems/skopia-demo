using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts;

public interface ITaskService :
    ICheckExistence<long>,
    IDeleteOperations<long>,
    IGetOperations<TaskResponseDTO, long>,
    IPostOperations<TaskRequestDTO, TaskResponseDTO>,
    IUpdateOperations<TaskUpdateRequestDTO, TaskResponseDTO>
{
    Task<bool> LimitExceeded(long id);
}
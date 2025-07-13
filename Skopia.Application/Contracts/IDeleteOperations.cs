using Skopia.Domain.Models;

namespace Skopia.Application.Contracts
{
    public interface IDeleteOperations<TId>
    {
        Task<OperationResultModel> DeleteAsync(TId id);
    }
}
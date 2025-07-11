namespace Skopia.Domain.Models
{
    public class OperationResultModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResultModel Ok() => new() { Success = true };
        public static OperationResultModel Fail(string error) => new() { Success = false, ErrorMessage = error };
    }

    public class OperationResultModel<T> : OperationResultModel
    {
        public T Data { get; set; }

        public static OperationResultModel<T> Ok(T data) =>
            new() { Success = true, Data = data };

        public new static OperationResultModel<T> Fail(string error) =>
            new() { Success = false, ErrorMessage = error };
    }
}
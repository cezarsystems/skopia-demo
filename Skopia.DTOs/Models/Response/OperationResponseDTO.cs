namespace Skopia.DTOs.Models.Response
{
    public class OperationResponseDTO
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResponseDTO Ok() => new() { Success = true };
        public static OperationResponseDTO Fail(string error) => new() { Success = false, ErrorMessage = error };
    }

    public class OperationResponseDTO<T> : OperationResponseDTO
    {
        public T Data { get; set; }

        public static OperationResponseDTO<T> Ok(T data) =>
            new() { Success = true, Data = data };

        public new static OperationResponseDTO<T> Fail(string error) =>
            new() { Success = false, ErrorMessage = error };
    }
}
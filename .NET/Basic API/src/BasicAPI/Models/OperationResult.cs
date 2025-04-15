
namespace BasicAPI.Models
{
    public class OperationResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

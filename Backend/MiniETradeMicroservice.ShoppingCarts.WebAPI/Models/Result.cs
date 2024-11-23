namespace MiniETradeMicroservice.Products.WebAPI.Models
{
    public class Result
    {
        public Result() { }

        public Result(bool status, string message)
        {
            Status = status;
            Message = message;
        }
        public Result(bool status)
        {
            Status = status;
        }

        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class DataResult<T> : Result
    {
        public DataResult() { }

        public DataResult(string message, bool status, T data)
        {
            Message = message;
            Status = status;
            Data = data;
        }
        public DataResult(bool status, T data)
        {
            Status = status;
            Data = data;
        }
        public T Data { get; set; }
    }
}

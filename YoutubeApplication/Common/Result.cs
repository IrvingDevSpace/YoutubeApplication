namespace YoutubeApplication.Common
{
    public class Result
    {
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }

        public static Result Success() => new() { IsSuccess = true };
        public static Result Failure(string msg) => new() { IsSuccess = false, Message = msg };
    }

    public class Result<T> : Result
    {
        public T? Data { get; init; }

        public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };

        new public static Result<T> Failure(string msg) => new() { IsSuccess = false, Message = msg };
    }
}

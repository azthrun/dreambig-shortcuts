namespace DreamBig.Shortcuts.Results;

public sealed class Result<T> where T : class
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; init; }
    public int? StatusCode { get; init; }
    public ErrorInfo? Error { get; init; }

    private Result(bool isSuccess, T? value, ErrorInfo? error, int? statusCode = null)
    {
        if (isSuccess is true && error is not null)
        {
            throw new ArgumentException("A successful result cannot have an error.");
        }

        if (isSuccess is false && error is null)
        {
            throw new ArgumentException("A failure result must have an error.");
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value, int? httpStatusCode = null)
        => new(true, value, null, httpStatusCode);

    public static Result<T> Failure(string error, int? httpStatusCode = null)
        => new(false, null, new ErrorInfo { Message = error }, httpStatusCode);

    public static Result<T> Failure(Exception exception, int? httpStatusCode = null)
        => new(false, null, new ErrorInfo { Exception = exception }, httpStatusCode);

    public static Result<T> Failure(string message, Exception exception, int? httpStatusCode = null)
        => new(false, null, new ErrorInfo { Message = message, Exception = exception }, httpStatusCode);

    public class ErrorInfo
    {
        public string? Message { get; set; }
        public Exception? Exception { get; set; }

        public static implicit operator Result<T>(ErrorInfo error)
            => Result<T>.Failure(error.Message ?? string.Empty, error.Exception ?? new Exception("Unknown error"));
    }
}
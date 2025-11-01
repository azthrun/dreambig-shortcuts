namespace DreamBig.Shortcuts.Results;

/// <summary>
/// Represents the result of an operation that can either succeed or fail.
/// </summary>
/// <typeparam name="T">The type of the success value.</typeparam>
/// <remarks>Revision: 1.0</remarks>
public sealed class Result<T> where T : class
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the value returned by a successful operation, or null if the operation failed.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public T? Value { get; init; }

    /// <summary>
    /// Gets the HTTP status code associated with the result, if applicable.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public int? StatusCode { get; init; }

    /// <summary>
    /// Gets the error information if the operation failed, or null if the operation succeeded.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public ErrorInfo? Error { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="value">The value returned by a successful operation.</param>
    /// <param name="error">The error information if the operation failed.</param>
    /// <param name="statusCode">The HTTP status code associated with the result.</param>
    /// <exception cref="ArgumentException">Thrown when a successful result has an error or when a failure result doesn't have an error.</exception>
    /// <remarks>Revision: 1.0</remarks>
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

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <param name="httpStatusCode">The optional HTTP status code.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the specified value.</returns>
    /// <remarks>Revision: 1.0</remarks>
    public static Result<T> Success(T value, int? httpStatusCode = null)
        => new(true, value, null, httpStatusCode);

    /// <summary>
    /// Creates a failed result with the specified error message.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <param name="httpStatusCode">The optional HTTP status code.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the error message.</returns>
    /// <remarks>Revision: 1.0</remarks>
    public static Result<T> Failure(string error, int? httpStatusCode = null)
        => new(false, null, new ErrorInfo { Message = error }, httpStatusCode);

    /// <summary>
    /// Creates a failed result with the specified exception.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="httpStatusCode">The optional HTTP status code.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the exception.</returns>
    /// <remarks>Revision: 1.0</remarks>
    public static Result<T> Failure(Exception exception, int? httpStatusCode = null)
        => new(false, null, new ErrorInfo { Exception = exception }, httpStatusCode);

    /// <summary>
    /// Creates a failed result with the specified error message and exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="httpStatusCode">The optional HTTP status code.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the error message and exception.</returns>
    /// <remarks>Revision: 1.0</remarks>
    public static Result<T> Failure(string message, Exception exception, int? httpStatusCode = null)
        => new(false, null, new ErrorInfo { Message = message, Exception = exception }, httpStatusCode);

    /// <summary>
    /// Implicitly converts an <see cref="ErrorInfo"/> to a failed <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="error">The error information.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the error information.</returns>
    /// <remarks>Revision: 1.0</remarks>
    public static implicit operator Result<T>(ErrorInfo error)
        => Result<T>.Failure(error.Message ?? string.Empty, error.Exception ?? new Exception("Unknown error"));
}
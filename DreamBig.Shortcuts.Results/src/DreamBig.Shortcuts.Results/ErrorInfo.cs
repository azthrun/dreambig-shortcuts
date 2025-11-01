namespace DreamBig.Shortcuts.Results;

/// <summary>
/// Represents error information for a failed result.
/// </summary>
/// <remarks>Revision: 1.0</remarks>
public class ErrorInfo
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the exception associated with the error.
    /// </summary>
    /// <remarks>Revision: 1.0</remarks>
    public Exception? Exception { get; set; }
}

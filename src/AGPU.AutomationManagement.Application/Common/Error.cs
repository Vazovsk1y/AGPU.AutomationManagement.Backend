namespace AGPU.AutomationManagement.Application.Common;

public sealed record Error
{
    public static readonly Error None = new(string.Empty);

    public string Message { get; }

    public Error(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Message = message;
    }

    public static implicit operator Error(string message) => new(message);
}
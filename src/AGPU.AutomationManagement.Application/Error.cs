namespace AGPU.AutomationManagement.Application;

public sealed record Error
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public string Code { get; }
    
    public string Message { get; }

    public Error(string code, string message)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(message);
        Message = message;
        Code = code;
    }

    public static implicit operator Error((string code, string message) value) => new(value.code, value.message);
}
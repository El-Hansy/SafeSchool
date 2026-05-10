namespace SafeSchool.Api.Features.Transport.Common;

public sealed record ValidationError(string Code, string Message, string? Field = null);
public sealed record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
public sealed record TransportSourceMetadata(string ActorReference, string DeviceReference, string SourceIp = "", string UserAgent = "");

public interface ITransportClock
{
    DateTimeOffset UtcNow { get; }
}

public sealed class SystemTransportClock : ITransportClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

public sealed class OperationResult<T>
{
    private OperationResult(T? value, IReadOnlyList<ValidationError> errors)
    {
        Value = value;
        Errors = errors;
    }

    public bool Succeeded => Errors.Count == 0;
    public T? Value { get; }
    public IReadOnlyList<ValidationError> Errors { get; }

    public static OperationResult<T> Success(T value) => new(value, []);
    public static OperationResult<T> Failure(params ValidationError[] errors) => new(default, errors);
}

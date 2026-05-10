namespace SafeSchool.Api.Features.IdentityAccess.Common;

public sealed record ValidationError(string Code, string Message, string? Target = null);

public sealed record OperationResult<T>(bool Succeeded, T? Value, IReadOnlyList<ValidationError> Errors)
{
    public static OperationResult<T> Success(T value) => new(true, value, []);
    public static OperationResult<T> Failure(params ValidationError[] errors) => new(false, default, errors);
}

public sealed record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);

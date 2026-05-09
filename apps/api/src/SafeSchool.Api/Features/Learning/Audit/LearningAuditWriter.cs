using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Audit;

public interface ILearningAuditWriter
{
    Task<OperationResult<LearningAuditEvent>> RecordAsync(LearningAuditEvent auditEvent, CancellationToken cancellationToken = default);
}

public sealed class LearningAuditWriter : ILearningAuditWriter
{
    private readonly List<LearningAuditEvent> events = [];

    public IReadOnlyList<LearningAuditEvent> Events => events;

    public Task<OperationResult<LearningAuditEvent>> RecordAsync(LearningAuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(auditEvent.EventType))
        {
            return Task.FromResult(OperationResult<LearningAuditEvent>.Failure(new ValidationError("audit_failure", "Learning audit event type is required.", nameof(auditEvent.EventType))));
        }

        events.Add(auditEvent);
        return Task.FromResult(OperationResult<LearningAuditEvent>.Success(auditEvent));
    }
}

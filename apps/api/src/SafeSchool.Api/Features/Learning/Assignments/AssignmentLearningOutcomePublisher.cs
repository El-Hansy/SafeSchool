using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Common.Trace;

namespace SafeSchool.Api.Features.Learning.Assignments;

public sealed class AssignmentLearningOutcomePublisher
{
    public OperationResult<LearningCommandResult> Execute(string tenantId = "school-1", string actorReference = "actor-1", string sourceReference = "demo")
    {
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return OperationResult<LearningCommandResult>.Failure(new ValidationError("tenant_required", "A school account is required."));
        }

        return OperationResult<LearningCommandResult>.Success(new LearningCommandResult("assignmentlearningoutcomepublisher", [new LearningTraceReference("learning", sourceReference, tenantId, actorReference)]));
    }
}

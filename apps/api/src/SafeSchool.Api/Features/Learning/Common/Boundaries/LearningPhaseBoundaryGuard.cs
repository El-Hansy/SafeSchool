using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Common.Boundaries;

public sealed class LearningPhaseBoundaryGuard
{
    private static readonly HashSet<string> ProhibitedSideEffects = new(StringComparer.OrdinalIgnoreCase)
    {
        "attendance", "campus", "campus_gate", "nfc", "qr", "scan", "transport", "wallet", "payment", "refund", "canteen_purchase", "request_approval", "medical", "emergency", "complaint", "discipline_case", "document", "search", "dashboard", "message", "messaging", "broadcast", "notification_delivery"
    };

    public IReadOnlyCollection<string> BlockedSideEffects => ProhibitedSideEffects;

    public OperationResult<string> EnsureNoOutOfScopeSideEffect(string sideEffect)
    {
        if (ProhibitedSideEffects.Contains(sideEffect))
        {
            return OperationResult<string>.Failure(new ValidationError("phase_boundary_violation", $"Phase 5 cannot create {sideEffect} outcomes.", sideEffect));
        }

        return OperationResult<string>.Success(sideEffect);
    }
}

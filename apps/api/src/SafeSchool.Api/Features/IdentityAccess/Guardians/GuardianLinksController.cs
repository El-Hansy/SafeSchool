using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public static class GuardianLinksController
{
    public static RouteGroupBuilder MapGuardianLinkEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/students/{studentProfileId:guid}/guardian-links", async (string schoolAccountId, Guid studentProfileId, GuardianLinkRequest request, GuardianLinkService service, CancellationToken ct) =>
            Results.Created("/guardian-links", await service.CreateAsync(schoolAccountId, studentProfileId, request, ct)));
        group.MapPatch("/guardian-links/{guardianLinkId:guid}", async (string schoolAccountId, Guid guardianLinkId, GuardianLinkRequest request, GuardianLinkService service, CancellationToken ct) =>
            Results.Ok(await service.UpdateScopeAsync(schoolAccountId, guardianLinkId, request, ct)));
        group.MapPost("/guardian-links/{guardianLinkId:guid}/approve", async (string schoolAccountId, Guid guardianLinkId, GuardianLinkStateRequest request, GuardianLinkService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, guardianLinkId, GuardianLinkStatus.Approved, request.ReviewReason, ct)));
        group.MapPost("/guardian-links/{guardianLinkId:guid}/suspend", async (string schoolAccountId, Guid guardianLinkId, GuardianLinkStateRequest request, GuardianLinkService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, guardianLinkId, GuardianLinkStatus.Suspended, request.ReviewReason, ct)));
        group.MapPost("/guardian-links/{guardianLinkId:guid}/remove", async (string schoolAccountId, Guid guardianLinkId, GuardianLinkStateRequest request, GuardianLinkService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, guardianLinkId, GuardianLinkStatus.Removed, request.ReviewReason, ct)));
        group.MapPost("/guardian-links/{guardianLinkId:guid}/expire", async (string schoolAccountId, Guid guardianLinkId, GuardianLinkStateRequest request, GuardianLinkService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, guardianLinkId, GuardianLinkStatus.Expired, request.ReviewReason, ct)));
        group.MapGet("/guardian-links/{guardianLinkId:guid}/history", async (string schoolAccountId, Guid guardianLinkId, GuardianLinkService service, CancellationToken ct) =>
            Results.Ok(await service.HistoryAsync(schoolAccountId, guardianLinkId, ct)));
        return group;
    }
}

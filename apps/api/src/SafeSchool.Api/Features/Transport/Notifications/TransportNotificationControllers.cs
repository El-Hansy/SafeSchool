namespace SafeSchool.Api.Features.Transport.Notifications;

using SafeSchool.Api.Infrastructure.Tenancy;

public static class TransportNotificationControllers
{
    public static RouteGroupBuilder MapTransportNotificationEndpoints(this RouteGroupBuilder group, RouteGroupBuilder guardianGroup)
    {
        var records = group.MapGroup("/notification-records");
        records.MapPost("/{notificationRecordId:guid}/withdraw", async (string schoolAccountId, Guid notificationRecordId, WithdrawNotificationRequest request, TransportNotificationWithdrawalService service, CancellationToken ct) =>
        {
            var result = await service.WithdrawAsync(schoolAccountId, notificationRecordId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        records.MapGet("/{notificationRecordId:guid}/trace", async (string schoolAccountId, Guid notificationRecordId, TransportNotificationTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, notificationRecordId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        guardianGroup.MapGet("/{studentProfileId}/transport/notifications", async (string studentProfileId, ITenantContext tenantContext, GuardianTransportNotificationVisibilityService service, CancellationToken ct) =>
        {
            var result = await service.ListAsync(GuardianTenantResolver.Resolve(tenantContext), tenantContext.ActorReference ?? "anonymous", studentProfileId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}

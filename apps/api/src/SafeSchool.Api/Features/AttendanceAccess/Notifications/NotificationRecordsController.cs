using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public static class NotificationRecordsController
{
    public static RouteGroupBuilder MapNotificationEndpoints(this RouteGroupBuilder group)
    {
        var notifications = group.MapGroup("/notifications");
        notifications.MapGet("/", async (string schoolAccountId, SafeSchoolDbContext dbContext, CancellationToken ct) =>
            Results.Ok(await dbContext.EntryExitNotificationRecords.Where(x => x.TenantId == schoolAccountId).Select(x => x.ToResponse()).ToListAsync(ct)));
        notifications.MapPost("/{notificationId:guid}/withdraw", async (string schoolAccountId, Guid notificationId, WithdrawNotificationRequest request, EntryExitNotificationService service, CancellationToken ct) =>
        {
            var result = await service.WithdrawAsync(schoolAccountId, notificationId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapGuardianEntryExitEndpoints();
        return group;
    }
}


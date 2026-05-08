using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public static class AttendanceControllers
{
    public static RouteGroupBuilder MapAttendanceEndpoints(this RouteGroupBuilder group)
    {
        var sessions = group.MapGroup("/attendance/sessions");
        sessions.MapPost("/", async (string schoolAccountId, CreateAttendanceSessionRequest request, AttendanceSessionService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/attendance/sessions/{result.Value!.AttendanceSessionId}", result.Value) : Results.BadRequest(result.Errors);
        });
        sessions.MapGet("/", async (string schoolAccountId, AttendanceSessionService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        sessions.MapPost("/{sessionId:guid}/generate", async (string schoolAccountId, Guid sessionId, GenerateAttendanceRequest request, AttendanceGenerationService service, CancellationToken ct) =>
            Results.Ok(await service.GenerateAsync(schoolAccountId, sessionId, request, ct)));

        var records = group.MapGroup("/attendance/records");
        records.MapGet("/", async (string schoolAccountId, SafeSchoolDbContext dbContext, CancellationToken ct) =>
            Results.Ok(await dbContext.AttendanceRecords.Where(x => x.TenantId == schoolAccountId).Select(x => x.ToResponse()).ToListAsync(ct)));
        records.MapPost("/{recordId:guid}/correct", async (string schoolAccountId, Guid recordId, CorrectAttendanceRequest request, AttendanceCorrectionService service, CancellationToken ct) =>
        {
            var result = await service.CorrectAsync(schoolAccountId, recordId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapGet("/attendance/sessions/{sessionId:guid}/summary", async (string schoolAccountId, Guid sessionId, AttendanceSummaryService service, CancellationToken ct) =>
            Results.Ok(await service.SummaryAsync(schoolAccountId, sessionId, ct)));
        return group;
    }
}


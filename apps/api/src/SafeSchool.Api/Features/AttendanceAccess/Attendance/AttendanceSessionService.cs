using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceSessionService(SafeSchoolDbContext dbContext, IExpectedStudentPopulationProvider populationProvider)
{
    public async Task<OperationResult<AttendanceSessionResponse>> CreateAsync(string tenantId, CreateAttendanceSessionRequest request, CancellationToken cancellationToken = default)
    {
        var expected = await populationProvider.GetExpectedStudentsAsync(tenantId, request.ExpectedPopulationRule, cancellationToken);
        if (expected.Count == 0)
        {
            return OperationResult<AttendanceSessionResponse>.Failure(new ValidationError("empty_population", "Attendance session requires an expected student population."));
        }

        var session = new AttendanceSession
        {
            TenantId = tenantId,
            SessionName = request.SessionName,
            AttendanceDate = request.AttendanceDate,
            CampusReference = request.CampusReference,
            ExpectedPopulationRule = request.ExpectedPopulationRule,
            EntryWindowStart = request.EntryWindowStart,
            EntryWindowEnd = request.EntryWindowEnd,
            LateAfter = request.LateAfter,
            EarlyExitBefore = request.EarlyExitBefore,
            GenerationStatus = AttendanceSessionStatus.Active
        };
        if (!session.IsValidWindow())
        {
            return OperationResult<AttendanceSessionResponse>.Failure(new ValidationError("invalid_window", "Attendance session windows are not ordered correctly."));
        }

        dbContext.AttendanceSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<AttendanceSessionResponse>.Success(session.ToResponse());
    }

    public async Task<IReadOnlyList<AttendanceSessionResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.AttendanceSessions.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.AttendanceDate).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
}


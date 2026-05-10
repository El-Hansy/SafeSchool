using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceGenerationService(
    SafeSchoolDbContext dbContext,
    IExpectedStudentPopulationProvider populationProvider,
    AttendanceRuleEvaluator ruleEvaluator)
{
    public async Task<IReadOnlyList<AttendanceRecordResponse>> GenerateAsync(string tenantId, Guid sessionId, GenerateAttendanceRequest request, CancellationToken cancellationToken = default)
    {
        var session = await dbContext.AttendanceSessions.SingleAsync(x => x.TenantId == tenantId && x.Id == sessionId, cancellationToken);
        var expectedStudents = await populationProvider.GetExpectedStudentsAsync(tenantId, session.ExpectedPopulationRule, cancellationToken);
        var date = session.AttendanceDate;
        var scans = await dbContext.GateScanEvents
            .Where(x => x.TenantId == tenantId && DateOnly.FromDateTime(x.LocalScanTime.UtcDateTime) == date)
            .OrderBy(x => x.LocalScanTime)
            .ToListAsync(cancellationToken);

        foreach (var student in expectedStudents)
        {
            var entry = scans.FirstOrDefault(x => x.StudentProfileId == student && x.Direction == AttendanceDirection.Entry);
            var exit = scans.FirstOrDefault(x => x.StudentProfileId == student && x.Direction == AttendanceDirection.Exit);
            var status = ruleEvaluator.Evaluate(session, entry, exit);
            var record = await dbContext.AttendanceRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.AttendanceSessionId == sessionId && x.StudentProfileId == student, cancellationToken);
            if (record is null)
            {
                record = new AttendanceRecord { TenantId = tenantId, AttendanceSessionId = sessionId, StudentProfileId = student };
                dbContext.AttendanceRecords.Add(record);
            }

            record.Status = status;
            record.SourceScanEventId = entry?.Id;
            record.UpdatedAt = DateTimeOffset.UtcNow;
        }

        session.GenerationStatus = AttendanceSessionStatus.Generated;
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.AttendanceRecords.Where(x => x.TenantId == tenantId && x.AttendanceSessionId == sessionId).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    }
}


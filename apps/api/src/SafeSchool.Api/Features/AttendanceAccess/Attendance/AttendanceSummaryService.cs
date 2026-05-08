using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceSummaryService(SafeSchoolDbContext dbContext)
{
    public async Task<AttendanceSummaryResponse> SummaryAsync(string tenantId, Guid sessionId, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.AttendanceRecords.Where(x => x.TenantId == tenantId && x.AttendanceSessionId == sessionId).ToListAsync(cancellationToken);
        return new(sessionId,
            records.Count(x => x.Status == AttendanceStatus.Present),
            records.Count(x => x.Status == AttendanceStatus.Late),
            records.Count(x => x.Status == AttendanceStatus.Absent),
            records.Count(x => x.Status == AttendanceStatus.EarlyExit),
            records.Count(x => x.Status == AttendanceStatus.NeedsReview));
    }
}


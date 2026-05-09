using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceCorrectionService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<AttendanceRecordResponse>> CorrectAsync(string tenantId, Guid recordId, CorrectAttendanceRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return OperationResult<AttendanceRecordResponse>.Failure(new ValidationError("missing_reason", "Attendance correction requires a reason."));
        }

        var record = await dbContext.AttendanceRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == recordId, cancellationToken);
        if (record is null)
        {
            return OperationResult<AttendanceRecordResponse>.Failure(new ValidationError("not_found", "Attendance record was not found in this school account."));
        }

        record.Status = request.Status;
        record.CorrectionReason = request.Reason;
        record.AnomalyId = request.AnomalyId;
        record.ManualReviewId = request.ManualReviewId;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<AttendanceRecordResponse>.Success(record.ToResponse());
    }
}


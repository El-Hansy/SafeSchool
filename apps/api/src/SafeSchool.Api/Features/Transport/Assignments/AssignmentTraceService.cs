using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common.Trace;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class AssignmentTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<AssignmentTraceResponse?> TraceAsync(string tenantId, Guid assignmentId, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.StudentTransportAssignments.AnyAsync(x => x.TenantId == tenantId && x.Id == assignmentId, cancellationToken);
        return exists ? new AssignmentTraceResponse(assignmentId, [new TransportTraceReference("Assignment", assignmentId.ToString(), "Student assignment"), new TransportTraceReference("Scans", assignmentId.ToString(), "Boarding and drop scans")]) : null;
    }
}

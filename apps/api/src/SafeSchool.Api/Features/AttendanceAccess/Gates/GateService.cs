using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Audit;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Gates;

public sealed class GateService(SafeSchoolDbContext dbContext, AttendanceAccessPermissionGuard guard, IAttendanceAccessAuditWriter auditWriter)
{
    public async Task<OperationResult<GateResponse>> CreateAsync(string tenantId, CreateGateRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, AttendanceAccessCapabilities.GateScanning, AttendanceAccessPermissionCatalog.GatesManage, targetType: "Gate", targetReference: request.GateCode, cancellationToken: cancellationToken);
        if (!allowed.Succeeded)
        {
            return OperationResult<GateResponse>.Failure(allowed.Errors.ToArray());
        }

        var gate = new Gate
        {
            TenantId = tenantId,
            GateCode = request.GateCode,
            DisplayName = request.DisplayName,
            CampusReference = request.CampusReference,
            AllowsEntry = request.AllowsEntry,
            AllowsExit = request.AllowsExit,
            OfflineAllowed = request.OfflineAllowed,
            Status = GateStatus.Active
        };
        dbContext.Gates.Add(gate);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new AttendanceAccessAuditEvent { TenantId = tenantId, EventCategory = "Gate", EventType = "attendance_access.gates.create", SubjectType = "Gate", SubjectReference = gate.Id.ToString(), Reason = "Gate created" }, cancellationToken);
        return OperationResult<GateResponse>.Success(gate.ToResponse());
    }

    public async Task<IReadOnlyList<GateResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.Gates.Where(x => x.TenantId == tenantId).OrderBy(x => x.GateCode).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<OperationResult<GateResponse>> UpdateAsync(string tenantId, Guid gateId, UpdateGateRequest request, CancellationToken cancellationToken = default)
    {
        var gate = await dbContext.Gates.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == gateId, cancellationToken);
        if (gate is null)
        {
            return OperationResult<GateResponse>.Failure(new ValidationError("not_found", "Gate was not found in this school account."));
        }

        if (request.Status is not null && !gate.CanTransitionTo(request.Status.Value))
        {
            return OperationResult<GateResponse>.Failure(new ValidationError("invalid_transition", "Gate cannot move to the requested status."));
        }

        gate.DisplayName = request.DisplayName ?? gate.DisplayName;
        gate.Status = request.Status ?? gate.Status;
        gate.AllowsEntry = request.AllowsEntry ?? gate.AllowsEntry;
        gate.AllowsExit = request.AllowsExit ?? gate.AllowsExit;
        gate.OfflineAllowed = request.OfflineAllowed ?? gate.OfflineAllowed;
        gate.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new AttendanceAccessAuditEvent { TenantId = tenantId, EventCategory = "Gate", EventType = "attendance_access.gates.update", SubjectType = "Gate", SubjectReference = gate.Id.ToString(), Reason = request.Reason }, cancellationToken);
        return OperationResult<GateResponse>.Success(gate.ToResponse());
    }
}

public static class GateMapping
{
    public static GateResponse ToResponse(this Gate gate) => new(gate.Id, gate.TenantId, gate.GateCode, gate.DisplayName, gate.CampusReference, gate.Status, gate.AllowsEntry, gate.AllowsExit, gate.OfflineAllowed);
}


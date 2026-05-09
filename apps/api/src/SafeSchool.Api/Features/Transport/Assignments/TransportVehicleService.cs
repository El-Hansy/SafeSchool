using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class TransportVehicleService(SafeSchoolDbContext dbContext, TransportPermissionGuard guard)
{
    public async Task<OperationResult<VehicleResponse>> CreateAsync(string tenantId, CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, TransportCapabilities.BusAssignment, TransportPermissionCatalog.VehiclesManage, targetType: "TransportVehicle", targetReference: request.VehicleCode, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<VehicleResponse>.Failure(allowed.Errors.ToArray());
        if (request.Capacity < 0) return OperationResult<VehicleResponse>.Failure(new ValidationError("invalid_capacity", "Vehicle capacity cannot be negative."));
        var vehicle = new TransportVehicle { TenantId = tenantId, VehicleName = request.VehicleName, VehicleCode = request.VehicleCode, PlateReference = request.PlateReference, Capacity = request.Capacity, VehicleStatus = request.VehicleStatus, DefaultDriverReference = request.DefaultDriverReference, DefaultSupervisorReference = request.DefaultSupervisorReference };
        dbContext.TransportVehicles.Add(vehicle);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<VehicleResponse>.Success(vehicle.ToResponse());
    }

    public async Task<IReadOnlyList<VehicleResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.TransportVehicles.Where(x => x.TenantId == tenantId).OrderBy(x => x.VehicleCode).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
}

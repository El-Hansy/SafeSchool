using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class POSTerminalService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<PosTerminalResponse>> CreateAsync(string tenantId, PosTerminalRequest request, CancellationToken cancellationToken = default)
    {
        if (!await dbContext.CanteenMerchants.AnyAsync(x => x.TenantId == tenantId && x.Id == request.CanteenMerchantId && x.MerchantStatus == MerchantStatus.Active, cancellationToken)) return OperationResult<PosTerminalResponse>.Failure(new ValidationError("merchant_not_active", "Active POS terminals require an active merchant."));
        var terminal = new POSTerminal { TenantId = tenantId, CanteenMerchantId = request.CanteenMerchantId, TerminalCode = request.TerminalCode, DeviceReference = request.DeviceReference, OperatorReference = request.OperatorReference, OfflineEnabled = request.OfflineEnabled, PerTerminalReserveMinor = request.PerTerminalReserveMinor, Status = request.Status };
        dbContext.PosTerminals.Add(terminal);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<PosTerminalResponse>.Success(terminal.ToResponse());
    }
}

using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Wallets;

public sealed class GuardianWalletVisibilityService(SafeSchoolDbContext dbContext, IWalletGuardianLinkProvider guardianLinks)
{
    public async Task<OperationResult<WalletResponse>> GetAsync(string tenantId, string guardianActorId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var link = await guardianLinks.GetAsync(tenantId, guardianActorId, studentProfileId, cancellationToken);
        if (link.Status != WalletGuardianLinkStatus.Approved)
        {
            return OperationResult<WalletResponse>.Failure(new ValidationError("guardian_scope_denied", "Guardian cannot view this student wallet."));
        }

        var wallet = await dbContext.StudentWallets
            .Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId && x.WalletStatus != WalletStatus.Closed)
            .OrderBy(x => x.WalletCode)
            .Select(x => x.ToResponse())
            .FirstOrDefaultAsync(cancellationToken);

        return wallet is null
            ? OperationResult<WalletResponse>.Failure(new ValidationError("wallet_not_found", "No active wallet was found for this student."))
            : OperationResult<WalletResponse>.Success(wallet);
    }
}

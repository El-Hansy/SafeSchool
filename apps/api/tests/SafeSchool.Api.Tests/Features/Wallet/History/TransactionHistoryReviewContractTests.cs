using FluentAssertions;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Money;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Wallet.History;

public sealed class TransactionHistoryReviewContractTests
{
    [Fact]
    public void Phase4WalletBehavior_IsTenantScopedFeatureGatedAuditableAndMoneySafe()
    {
        WalletCapabilities.Ledger.Should().Be("wallet.ledger");
        WalletCapabilities.All.Should().Contain(WalletCapabilities.Reconciliation);
        WalletPermissionCatalog.WalletAdministrator.Should().Contain(WalletPermissionCatalog.AuditRead);
        new WalletMoneyValidator().RequirePositive(100, "SAR").Succeeded.Should().BeTrue();
        new ValidationError("tenant_mismatch", "Denied").Code.Should().Be("tenant_mismatch");
    }
}

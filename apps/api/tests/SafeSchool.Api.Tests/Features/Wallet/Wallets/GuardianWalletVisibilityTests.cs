using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Tests.Features.Wallet.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Wallet.Wallets;

public sealed class GuardianWalletVisibilityTests
{
    [Fact]
    public async Task GetAsync_returns_only_the_requested_tenant_wallet_for_approved_guardian_link()
    {
        await using var dbContext = CreateDbContext();
        dbContext.StudentWallets.AddRange(
            new StudentWallet
            {
                TenantId = "school-live",
                StudentProfileId = "student-amina",
                WalletCode = "W-AMINA",
                AvailableBalanceMinor = 18500,
                WalletStatus = WalletStatus.Active,
            },
            new StudentWallet
            {
                TenantId = "school-other",
                StudentProfileId = "student-amina",
                WalletCode = "W-AMINA-OTHER",
                AvailableBalanceMinor = 999999,
                WalletStatus = WalletStatus.Active,
            });
        await dbContext.SaveChangesAsync();

        var service = new GuardianWalletVisibilityService(dbContext, new FakeWalletGuardianLinkProvider());

        var result = await service.GetAsync("school-live", "guardian-live", "student-amina");

        result.Succeeded.Should().BeTrue();
        result.Value!.TenantId.Should().Be("school-live");
        result.Value.AvailableBalanceMinor.Should().Be(18500);
    }

    [Fact]
    public async Task GetAsync_rejects_unapproved_guardian_links()
    {
        await using var dbContext = CreateDbContext();
        var service = new GuardianWalletVisibilityService(
            dbContext,
            new FakeWalletGuardianLinkProvider { Status = WalletGuardianLinkStatus.Suspended });

        var result = await service.GetAsync("school-live", "guardian-live", "student-amina");

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.Code == "guardian_scope_denied");
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        return new SafeSchoolDbContext(options);
    }
}

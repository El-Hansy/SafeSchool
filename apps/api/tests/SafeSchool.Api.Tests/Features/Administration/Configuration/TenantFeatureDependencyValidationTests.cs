using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Administration;

public sealed class TenantFeatureDependencyValidationTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.AdministrationContractsHold();
}

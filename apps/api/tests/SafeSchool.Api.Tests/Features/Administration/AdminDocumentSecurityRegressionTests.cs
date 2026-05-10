using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Administration;

public sealed class AdminDocumentSecurityRegressionTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.AdministrationContractsHold();
}

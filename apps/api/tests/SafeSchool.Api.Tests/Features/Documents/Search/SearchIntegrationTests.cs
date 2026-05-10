using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Documents;

public sealed class SearchIntegrationTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.DocumentContractsHold();
}

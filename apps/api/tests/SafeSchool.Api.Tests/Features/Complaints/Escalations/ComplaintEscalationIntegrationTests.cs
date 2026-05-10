using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Complaints;

public sealed class ComplaintEscalationIntegrationTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.ComplaintContractsHold();
}

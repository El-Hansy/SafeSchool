using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Communications;

public sealed class DeliveryAcknowledgementIntegrationTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.CommunicationContractsHold();
}

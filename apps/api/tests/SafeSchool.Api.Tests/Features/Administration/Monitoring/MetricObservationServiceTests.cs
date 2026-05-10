using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Administration;

public sealed class MetricObservationServiceTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.AdministrationContractsHold();
}

using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Complaints;

public sealed class ComplaintNoSideEffectTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.ComplaintContractsHold();
}

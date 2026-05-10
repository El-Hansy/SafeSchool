using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Complaints;

public sealed class ComplaintMigrationTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.ComplaintContractsHold();
}

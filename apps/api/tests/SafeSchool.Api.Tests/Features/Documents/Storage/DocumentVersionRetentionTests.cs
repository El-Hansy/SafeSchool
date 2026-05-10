using SafeSchool.Api.Tests.Support;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Documents;

public sealed class DocumentVersionRetentionTests
{
    [Fact]
    public void Feature_contract_holds() => FeatureContractAssertions.DocumentContractsHold();
}

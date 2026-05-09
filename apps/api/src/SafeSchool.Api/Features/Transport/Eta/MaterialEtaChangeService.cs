namespace SafeSchool.Api.Features.Transport.Eta;

public sealed record MaterialEtaChange(Guid EtaRecordId, bool IsMaterial, string Reason);

public sealed class MaterialEtaChangeService
{
    public MaterialEtaChange Evaluate(EtaRecord current, EtaRecord? previous, TimeSpan threshold)
    {
        if (current.EstimatedArrivalTime is null || previous?.EstimatedArrivalTime is null) return new(current.Id, false, "Missing ETA comparison point.");
        var isMaterial = (current.EstimatedArrivalTime.Value - previous.EstimatedArrivalTime.Value).Duration() >= threshold;
        return new(current.Id, isMaterial, isMaterial ? "ETA changed materially." : "ETA change below threshold.");
    }
}

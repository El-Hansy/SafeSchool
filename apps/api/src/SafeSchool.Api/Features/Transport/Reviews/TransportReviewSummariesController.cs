namespace SafeSchool.Api.Features.Transport.Reviews;

public static class TransportReviewSummariesController
{
    public static RouteGroupBuilder MapTransportReviewSummaryEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/review-summaries", async (string schoolAccountId, TransportReviewSummaryService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        return group;
    }
}

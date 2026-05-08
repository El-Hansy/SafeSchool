namespace SafeSchool.Api.Features.Transport.Reviews;

public static class ManualTransportReviewsController
{
    public static RouteGroupBuilder MapManualReviewEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/manual-reviews", async (string schoolAccountId, ManualTransportReviewRequest request, ManualTransportReviewService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}

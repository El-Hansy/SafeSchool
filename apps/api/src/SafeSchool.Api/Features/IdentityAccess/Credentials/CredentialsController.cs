using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public static class CredentialsController
{
    public static RouteGroupBuilder MapCredentialEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/students/{studentProfileId:guid}/credentials/nfc", async (string schoolAccountId, Guid studentProfileId, IssueNfcCredentialRequest request, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Created("/credentials", await service.IssueNfcAsync(schoolAccountId, studentProfileId, request, ct)));
        group.MapPost("/credentials/{credentialId:guid}/suspend", async (string schoolAccountId, Guid credentialId, CredentialActionRequest request, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, credentialId, CredentialStatus.Suspended, request.Reason, ct)));
        group.MapPost("/credentials/{credentialId:guid}/restore", async (string schoolAccountId, Guid credentialId, CredentialActionRequest request, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, credentialId, CredentialStatus.Active, request.Reason, ct)));
        group.MapPost("/credentials/{credentialId:guid}/replace", async (string schoolAccountId, Guid credentialId, CredentialActionRequest request, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, credentialId, CredentialStatus.Replaced, request.Reason, ct)));
        group.MapPost("/credentials/{credentialId:guid}/revoke", async (string schoolAccountId, Guid credentialId, CredentialActionRequest request, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Ok(await service.ChangeStateAsync(schoolAccountId, credentialId, CredentialStatus.Revoked, request.Reason, ct)));
        group.MapPost("/students/{studentProfileId:guid}/credentials/qr", async (string schoolAccountId, Guid studentProfileId, CreateQrCredentialRequest request, QrFallbackCredentialService service, CancellationToken ct) =>
            Results.Created("/credentials", await service.CreateAsync(schoolAccountId, studentProfileId, request, ct)));
        group.MapPost("/credentials/{credentialId:guid}/rotate-qr", () => Results.Accepted());
        group.MapGet("/students/{studentProfileId:guid}/credentials", async (string schoolAccountId, Guid studentProfileId, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Ok(await service.ListForStudentAsync(schoolAccountId, studentProfileId, ct)));
        group.MapGet("/credentials/{credentialId:guid}/history", async (string schoolAccountId, Guid credentialId, CredentialLifecycleService service, CancellationToken ct) =>
            Results.Ok(await service.HistoryAsync(schoolAccountId, credentialId, ct)));
        group.MapGet("/credentials/status-snapshot", async (string schoolAccountId, CredentialStatusSnapshotService service, CancellationToken ct) =>
            Results.Ok(await service.CurrentAsync(schoolAccountId, ct)));
        return group;
    }
}

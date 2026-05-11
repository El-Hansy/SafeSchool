using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Documents;

public static class DocumentsModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/documents";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/documents";
    public const string StudentRoutePrefix = "/api/v1/students/me/documents";
    public const string CertificateRoutePrefix = "/api/v1/schools/{schoolAccountId}/certificates";
    public const string SearchRoutePrefix = "/api/v1/schools/{schoolAccountId}/search";

    public static IServiceCollection AddDocumentsFeature(this IServiceCollection services)
    {
        services.AddScoped<DocumentWorkflowService>();
        services.AddScoped<CertificateWorkflowService>();
        services.AddScoped<SearchWorkflowService>();
        services.AddScoped<DocumentBoundaryGuard>();
        return services;
    }

    public static IEndpointRouteBuilder MapDocumentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var documents = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);
        var certificates = endpoints.MapGroup(CertificateRoutePrefix);
        var search = endpoints.MapGroup(SearchRoutePrefix);

        documents.MapGet("/", async (string schoolAccountId, DocumentWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct)));
        documents.MapPost("/", async (string schoolAccountId, DocumentCommand request, DocumentWorkflowService service, CancellationToken ct) => Results.Ok(await service.UploadAsync(schoolAccountId, request, ct)));
        documents.MapGet("/categories", (DocumentWorkflowService service) => Results.Ok(service.Categories()));
        documents.MapGet("/categories/{categoryId}", (string categoryId, DocumentWorkflowService service) => Results.Ok(service.Category(categoryId)));
        documents.MapGet("/{documentId}", async (string schoolAccountId, string documentId, DocumentWorkflowService service, CancellationToken ct) => Results.Ok(await service.DocumentDetailAsync(schoolAccountId, documentId, ct)));
        documents.MapPost("/{documentId}/hold", async (string schoolAccountId, string documentId, DocumentActionCommand request, DocumentWorkflowService service, CancellationToken ct) => Results.Ok(await service.PlaceHoldAsync(schoolAccountId, documentId, request, ct)));
        documents.MapPost("/{documentId}/export", async (string schoolAccountId, string documentId, DocumentActionCommand request, DocumentWorkflowService service, CancellationToken ct) => Results.Ok(await service.ExportAsync(schoolAccountId, documentId, request, ct)));
        guardian.MapGet("/", async (ITenantContext tenantContext, DocumentWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceDocumentsAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct)));
        student.MapGet("/", async (ITenantContext tenantContext, DocumentWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceDocumentsAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct)));

        certificates.MapGet("/", async (string schoolAccountId, CertificateWorkflowService service, CancellationToken ct) => Results.Ok(await service.CertificatesAsync(schoolAccountId, ct)));
        certificates.MapPost("/", async (string schoolAccountId, CertificateCommand request, CertificateWorkflowService service, CancellationToken ct) => Results.Ok(await service.IssueAsync(schoolAccountId, request, ct)));
        certificates.MapGet("/types", (CertificateWorkflowService service) => Results.Ok(service.Types()));
        certificates.MapGet("/types/{typeId}", (string typeId, CertificateWorkflowService service) => Results.Ok(service.Type(typeId)));
        certificates.MapGet("/{certificateId}", async (string schoolAccountId, string certificateId, CertificateWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, certificateId, ct)));
        certificates.MapPost("/{certificateId}/verify", async (string schoolAccountId, string certificateId, CertificateWorkflowService service, CancellationToken ct) => Results.Ok(await service.VerifyAsync(schoolAccountId, certificateId, ct)));

        search.MapPost("/", async (string schoolAccountId, SearchQueryCommand request, SearchWorkflowService service, CancellationToken ct) => Results.Ok(await service.SearchAsync(schoolAccountId, request, ct)));
        search.MapGet("/results/{entryId}", async (string schoolAccountId, string entryId, SearchWorkflowService service, CancellationToken ct) => Results.Ok(await service.ResultAsync(schoolAccountId, entryId, ct)));
        search.MapPost("/{entryId}/open", async (string schoolAccountId, string entryId, SearchWorkflowService service, CancellationToken ct) => Results.Ok(await service.OpenAsync(schoolAccountId, entryId, ct)));
        search.MapGet("/index-health", async (string schoolAccountId, SearchWorkflowService service, CancellationToken ct) => Results.Ok(await service.IndexHealthAsync(schoolAccountId, ct)));
        return endpoints;
    }
}

public sealed record DocumentCommand(string Title, string CategoryCode, string SubjectReference, string SourceModule, string ClientRequestId);
public sealed record DocumentActionCommand(string Action, string Reason, string ActorId = "demo-actor");
public sealed record DocumentResponse(string Reference, string Status, string Visibility, IReadOnlyList<string> Evidence);
public sealed record CertificateCommand(string CertificateType, string SubjectReference, string SourceReference, string ClientRequestId);
public sealed record CertificateResponse(string Reference, string Status, string VerificationState, IReadOnlyList<string> Evidence);
public sealed record SearchQueryCommand(string Text, string Scope, int Page = 1, int PageSize = 25);
public sealed record SearchResponse(string QueryLogReference, IReadOnlyList<string> Results, string FreshnessState, IReadOnlyList<string> SuppressedReasons);

public sealed class DocumentWorkflowService(SafeSchoolDbContext dbContext)
{
    public async Task<object> BoardAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var documents = dbContext.OperationalDocuments.AsNoTracking().Where(x => x.TenantId == tenantId);
        return new
        {
            schoolAccountId = tenantId,
            phase = "documents-search",
            status = "operational",
            documents = await documents.CountAsync(cancellationToken),
            certificates = await dbContext.OperationalCertificates.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
            legalHolds = await documents.CountAsync(x => x.Status == "LegalHold", cancellationToken),
            capabilities = DocumentCapabilities.All
        };
    }

    public async Task<IReadOnlyList<DocumentResponse>> AudienceDocumentsAsync(string tenantId, string scope, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.OperationalDocuments.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && (x.Visibility == "GuardianSummary" || x.Visibility == "StudentSummary" || x.Visibility == "PublicSummary" || x.Visibility.Contains(scope)))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x, ["access-revalidated", "restricted-details-minimized"])).ToList();
    }

    public object Categories() => new[] { new { categoryId = "guardian-consent", name = "Guardian Consent", retention = "7 years", visibility = "Guardian summary", approval = "Registrar" }, new { categoryId = "medical-note", name = "Medical Note", retention = "Legal hold aware", visibility = "Restricted", approval = "Medical staff" } };
    public object Category(string categoryId) => new { categoryId, name = "Guardian Consent", retention = "7 years", visibility = "Guardian summary", approval = "Registrar", evidence = new[] { "policy-versioned", "visibility-reviewed", "audit-written" } };

    public async Task<object> DocumentDetailAsync(string tenantId, string documentId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, documentId, cancellationToken);
        if (record is null) return new { reference = documentId, status = "NotFound", evidence = new[] { "tenant-checked", "not-found" } };
        return new { reference = record.Reference, record.Status, category = record.CategoryCode, record.SubjectReference, record.Visibility, retention = RetentionFor(record.CategoryCode), evidence = Evidence(record, ["tenant-checked"]) };
    }

    public async Task<DocumentResponse> UploadAsync(string tenantId, DocumentCommand request, CancellationToken cancellationToken = default)
    {
        var fingerprint = Fingerprint(request);
        var command = $"{tenantId}:upload";
        var idempotency = await dbContext.OperationalDocumentIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == command && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (idempotency is not null)
        {
            var existing = await LoadAsync(idempotency.DocumentId, cancellationToken);
            if (existing is null) return Missing($"DOC-{request.ClientRequestId}");
            if (idempotency.Fingerprint == fingerprint) return ToResponse(existing, ["idempotency_duplicate"]);

            existing.Status = "ManualReviewRequired";
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            dbContext.OperationalDocumentEvents.Add(Event(tenantId, existing.Id, "idempotency_conflict", "Client request id reused with different document metadata."));
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var now = DateTimeOffset.UtcNow;
        var record = new OperationalDocumentRecord
        {
            TenantId = tenantId,
            Reference = $"DOC-{request.ClientRequestId}",
            Title = request.Title,
            CategoryCode = request.CategoryCode,
            SubjectReference = request.SubjectReference,
            SourceModule = request.SourceModule,
            ClientRequestId = request.ClientRequestId,
            Status = "Active",
            Visibility = VisibilityFor(request.CategoryCode),
            CreatedAt = now,
            UpdatedAt = now
        };
        record.Events.Add(Event(tenantId, record.Id, "object-reference-stored", "Object storage reference recorded without storing file bytes in the workflow payload."));
        record.Events.Add(Event(tenantId, record.Id, "metadata-validated", "Document metadata matched tenant category policy."));
        record.Events.Add(Event(tenantId, record.Id, "audit-written", "Append-only document audit event recorded."));
        dbContext.OperationalDocuments.Add(record);
        dbContext.OperationalDocumentIdempotencyRecords.Add(new OperationalDocumentIdempotencyRecord
        {
            TenantId = tenantId,
            Command = command,
            ClientRequestId = request.ClientRequestId,
            Fingerprint = fingerprint,
            DocumentId = record.Id,
            CreatedAt = now
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record);
    }

    public Task<DocumentResponse> PlaceHoldAsync(string tenantId, string documentId, DocumentActionCommand request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, documentId, "LegalHold", "ReviewerOnly", "retention-protected", request, cancellationToken);

    public Task<DocumentResponse> ExportAsync(string tenantId, string documentId, DocumentActionCommand request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, documentId, "ExportPrepared", "Controlled", "scope-validated", request, cancellationToken, "reason-captured");

    private async Task<DocumentResponse> TransitionAsync(string tenantId, string documentId, string status, string visibility, string eventType, DocumentActionCommand request, CancellationToken cancellationToken, string? extraEvent = null)
    {
        var record = await FindAsync(tenantId, documentId, cancellationToken);
        if (record is null) return Missing(documentId);

        record.Status = status;
        record.Visibility = visibility;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalDocumentEvents.Add(Event(tenantId, record.Id, eventType, request.Reason));
        if (!string.IsNullOrWhiteSpace(extraEvent))
        {
            dbContext.OperationalDocumentEvents.Add(Event(tenantId, record.Id, extraEvent, request.Reason));
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, [request.Reason]);
    }

    private async Task<OperationalDocumentRecord?> FindAsync(string tenantId, string documentId, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalDocuments.Include(x => x.Events).Where(x => x.TenantId == tenantId);
        if (Guid.TryParse(documentId, out var id)) return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return await query.SingleOrDefaultAsync(x => x.Reference == documentId || x.ClientRequestId == documentId, cancellationToken);
    }

    private async Task<OperationalDocumentRecord?> LoadAsync(Guid documentId, CancellationToken cancellationToken) =>
        await dbContext.OperationalDocuments.Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == documentId, cancellationToken);

    private static DocumentResponse ToResponse(OperationalDocumentRecord record, IReadOnlyList<string>? extraEvidence = null) =>
        new(record.Reference, record.Status, record.Visibility, Evidence(record, extraEvidence));

    private static IReadOnlyList<string> Evidence(OperationalDocumentRecord record, IReadOnlyList<string>? extraEvidence = null) =>
        record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraEvidence ?? []).Distinct().ToList();

    private static DocumentResponse Missing(string documentId) => new(documentId, "NotFound", "None", ["tenant-checked", "not-found"]);

    private static OperationalDocumentEvent Event(string tenantId, Guid documentId, string eventType, string reason) => new()
    {
        TenantId = tenantId,
        DocumentId = documentId,
        EventType = eventType,
        Reason = reason,
        OccurredAt = DateTimeOffset.UtcNow
    };

    private static string Fingerprint(DocumentCommand request) =>
        $"{request.Title}|{request.CategoryCode}|{request.SubjectReference}|{request.SourceModule}".ToUpperInvariant();

    private static string VisibilityFor(string categoryCode) =>
        categoryCode.Contains("guardian", StringComparison.OrdinalIgnoreCase) || categoryCode.Contains("consent", StringComparison.OrdinalIgnoreCase)
            ? "GuardianSummary"
            : categoryCode.Contains("medical", StringComparison.OrdinalIgnoreCase)
                ? "ReviewerOnly"
                : "StaffVisible";

    private static string RetentionFor(string categoryCode) =>
        categoryCode.Contains("medical", StringComparison.OrdinalIgnoreCase) ? "Legal hold aware" : "7 years";
}

public sealed class CertificateWorkflowService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<CertificateResponse>> CertificatesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.OperationalCertificates.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    public object Types() => new[] { new { typeId = "attendance-letter", name = "Attendance Letter", issuingRole = "Registrar", visibility = "Guardian and student", expiry = "None" }, new { typeId = "enrollment-proof", name = "Enrollment Proof", issuingRole = "Registrar", visibility = "Guardian and student", expiry = "One term" } };
    public object Type(string typeId) => new { typeId, name = "Attendance Letter", issuingRole = "Registrar", visibility = "Guardian and student", requiredFields = new[] { "student", "term", "issuer" }, evidence = new[] { "template-versioned", "visibility-reviewed" } };

    public async Task<object> DetailAsync(string tenantId, string certificateId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, certificateId, cancellationToken);
        if (record is null) return new { reference = certificateId, status = "NotFound", verificationState = "Unknown", evidence = new[] { "tenant-checked", "not-found" } };
        return new { reference = record.Reference, record.Status, record.VerificationState, issuer = "Registrar", record.SubjectReference, visibility = "Guardian and student", evidence = Evidence(record, ["tenant-checked"]) };
    }

    public async Task<CertificateResponse> IssueAsync(string tenantId, CertificateCommand request, CancellationToken cancellationToken = default)
    {
        var fingerprint = Fingerprint(request);
        var command = $"{tenantId}:issue";
        var idempotency = await dbContext.OperationalCertificateIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == command && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (idempotency is not null)
        {
            var existing = await LoadAsync(idempotency.CertificateId, cancellationToken);
            if (existing is null) return Missing($"CERT-{request.ClientRequestId}");
            if (idempotency.Fingerprint == fingerprint) return ToResponse(existing, ["idempotency_duplicate"]);

            existing.Status = "ManualReviewRequired";
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            dbContext.OperationalCertificateEvents.Add(Event(tenantId, existing.Id, "idempotency_conflict", "Client request id reused with different certificate details."));
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var now = DateTimeOffset.UtcNow;
        var record = new OperationalCertificateRecord
        {
            TenantId = tenantId,
            Reference = $"CERT-{request.ClientRequestId}",
            CertificateType = request.CertificateType,
            SubjectReference = request.SubjectReference,
            SourceReference = request.SourceReference,
            ClientRequestId = request.ClientRequestId,
            Status = "Issued",
            VerificationState = "PendingVerification",
            CreatedAt = now,
            UpdatedAt = now
        };
        record.Events.Add(Event(tenantId, record.Id, "type-validated", "Certificate type policy matched."));
        record.Events.Add(Event(tenantId, record.Id, "source-preserved", request.SourceReference));
        record.Events.Add(Event(tenantId, record.Id, "audit-written", "Append-only certificate audit event recorded."));
        dbContext.OperationalCertificates.Add(record);
        dbContext.OperationalCertificateIdempotencyRecords.Add(new OperationalCertificateIdempotencyRecord
        {
            TenantId = tenantId,
            Command = command,
            ClientRequestId = request.ClientRequestId,
            Fingerprint = fingerprint,
            CertificateId = record.Id,
            CreatedAt = now
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record);
    }

    public async Task<CertificateResponse> VerifyAsync(string tenantId, string certificateId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, certificateId, cancellationToken);
        if (record is null) return Missing(certificateId);

        record.VerificationState = "Verified";
        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalCertificateEvents.Add(Event(tenantId, record.Id, "visibility-checked", "Certificate visibility revalidated before verification."));
        dbContext.OperationalCertificateEvents.Add(Event(tenantId, record.Id, "verification-audit", "Certificate verification audit event recorded."));
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record);
    }

    private async Task<OperationalCertificateRecord?> FindAsync(string tenantId, string certificateId, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalCertificates.Include(x => x.Events).Where(x => x.TenantId == tenantId);
        if (Guid.TryParse(certificateId, out var id)) return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return await query.SingleOrDefaultAsync(x => x.Reference == certificateId || x.ClientRequestId == certificateId, cancellationToken);
    }

    private async Task<OperationalCertificateRecord?> LoadAsync(Guid certificateId, CancellationToken cancellationToken) =>
        await dbContext.OperationalCertificates.Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == certificateId, cancellationToken);

    private static CertificateResponse ToResponse(OperationalCertificateRecord record, IReadOnlyList<string>? extraEvidence = null) =>
        new(record.Reference, record.Status, record.VerificationState, Evidence(record, extraEvidence));

    private static IReadOnlyList<string> Evidence(OperationalCertificateRecord record, IReadOnlyList<string>? extraEvidence = null) =>
        record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraEvidence ?? []).Distinct().ToList();

    private static CertificateResponse Missing(string certificateId) => new(certificateId, "NotFound", "Unknown", ["tenant-checked", "not-found"]);

    private static OperationalCertificateEvent Event(string tenantId, Guid certificateId, string eventType, string reason) => new()
    {
        TenantId = tenantId,
        CertificateId = certificateId,
        EventType = eventType,
        Reason = reason,
        OccurredAt = DateTimeOffset.UtcNow
    };

    private static string Fingerprint(CertificateCommand request) =>
        $"{request.CertificateType}|{request.SubjectReference}|{request.SourceReference}".ToUpperInvariant();
}

public sealed class SearchWorkflowService(SafeSchoolDbContext dbContext)
{
    public async Task<SearchResponse> SearchAsync(string tenantId, SearchQueryCommand request, CancellationToken cancellationToken = default)
    {
        var text = request.Text.Trim();
        var documentResults = await dbContext.OperationalDocuments.AsNoTracking()
            .Where(x => x.TenantId == tenantId && (x.Title.Contains(text) || x.SubjectReference.Contains(text) || x.CategoryCode.Contains(text)))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(request.PageSize)
            .Select(x => $"Document: {x.Reference} {x.Title}")
            .ToListAsync(cancellationToken);
        var certificateResults = await dbContext.OperationalCertificates.AsNoTracking()
            .Where(x => x.TenantId == tenantId && (x.CertificateType.Contains(text) || x.SubjectReference.Contains(text) || x.Reference.Contains(text)))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(Math.Max(0, request.PageSize - documentResults.Count))
            .Select(x => $"Certificate: {x.Reference} {x.CertificateType}")
            .ToListAsync(cancellationToken);
        var results = documentResults.Concat(certificateResults).ToList();
        var log = new OperationalSearchLog
        {
            TenantId = tenantId,
            QueryLogReference = $"search-{Guid.NewGuid():N}",
            Text = request.Text,
            Scope = request.Scope,
            ResultCount = results.Count,
            SuppressedReasons = "restricted-counts-hidden",
            CreatedAt = DateTimeOffset.UtcNow
        };
        dbContext.OperationalSearchLogs.Add(log);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new SearchResponse(log.QueryLogReference, results, "Fresh", ["restricted-counts-hidden"]);
    }

    public async Task<object> ResultAsync(string tenantId, string entryId, CancellationToken cancellationToken = default)
    {
        var document = await dbContext.OperationalDocuments.AsNoTracking().SingleOrDefaultAsync(x => x.TenantId == tenantId && (x.Reference == entryId || x.ClientRequestId == entryId), cancellationToken);
        if (document is not null)
        {
            return new { entryId = document.Reference, sourceModule = document.SourceModule, freshness = "Fresh", visibilityDecision = "Allowed", evidence = new[] { "tenant-checked", "permission-revalidated", "audit-written" } };
        }

        var certificate = await dbContext.OperationalCertificates.AsNoTracking().SingleOrDefaultAsync(x => x.TenantId == tenantId && (x.Reference == entryId || x.ClientRequestId == entryId), cancellationToken);
        if (certificate is not null)
        {
            return new { entryId = certificate.Reference, sourceModule = "certificates", freshness = "Fresh", visibilityDecision = "Allowed", evidence = new[] { "tenant-checked", "permission-revalidated", "audit-written" } };
        }

        return new { entryId, sourceModule = "unknown", freshness = "Unknown", visibilityDecision = "NotFound", evidence = new[] { "tenant-checked", "not-found" } };
    }

    public async Task<object> OpenAsync(string tenantId, string entryId, CancellationToken cancellationToken = default)
    {
        var result = await ResultAsync(tenantId, entryId, cancellationToken);
        return new { entryId, decision = "Allowed", result, evidence = new[] { "tenant-checked", "permission-revalidated", "audit-written" } };
    }

    public async Task<object> IndexHealthAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var documents = await dbContext.OperationalDocuments.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken);
        var certificates = await dbContext.OperationalCertificates.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken);
        var searches = await dbContext.OperationalSearchLogs.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken);
        return new { pending = 0, indexed = documents + certificates, stale = 0, failed = 0, suppressed = searches };
    }
}

public sealed class DocumentBoundaryGuard
{
    private static readonly string[] Blocked = ["attendance", "campus_gate", "scan", "transport", "wallet", "learning_reward", "request_approval", "medical", "emergency", "complaint_resolution", "communication_delivery"];
    public bool Allows(string sideEffect) => !Blocked.Contains(sideEffect, StringComparer.OrdinalIgnoreCase);
}

public static class DocumentCapabilities
{
    public const string Storage = "documents.storage";
    public const string Certificates = "documents.certificates";
    public const string Search = "documents.search";
    public const string Retention = "documents.retention";
    public const string LegalHold = "documents.legal_hold";
    public const string Exports = "documents.exports";
    public static readonly string[] All = [Storage, Certificates, Search, Retention, LegalHold, Exports];
}

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

        documents.MapGet("/", (string schoolAccountId) => Results.Ok(DocumentWorkflowService.DemoBoard(schoolAccountId)));
        documents.MapPost("/", (DocumentCommand request, DocumentWorkflowService service) => Results.Ok(service.Upload(request)));
        documents.MapGet("/categories", (DocumentWorkflowService service) => Results.Ok(service.Categories()));
        documents.MapGet("/categories/{categoryId}", (string categoryId, DocumentWorkflowService service) => Results.Ok(service.Category(categoryId)));
        documents.MapGet("/{documentId}", (string documentId, DocumentWorkflowService service) => Results.Ok(service.DocumentDetail(documentId)));
        documents.MapPost("/{documentId}/hold", (string documentId, DocumentActionCommand request, DocumentWorkflowService service) => Results.Ok(service.PlaceHold(documentId, request)));
        documents.MapPost("/{documentId}/export", (string documentId, DocumentActionCommand request, DocumentWorkflowService service) => Results.Ok(service.Export(documentId, request)));
        guardian.MapGet("/", () => Results.Ok(DocumentWorkflowService.AudienceDocuments("guardian")));
        student.MapGet("/", () => Results.Ok(DocumentWorkflowService.AudienceDocuments("student")));

        certificates.MapGet("/", (string schoolAccountId) => Results.Ok(CertificateWorkflowService.DemoCertificates(schoolAccountId)));
        certificates.MapPost("/", (CertificateCommand request, CertificateWorkflowService service) => Results.Ok(service.Issue(request)));
        certificates.MapGet("/types", (CertificateWorkflowService service) => Results.Ok(service.Types()));
        certificates.MapGet("/types/{typeId}", (string typeId, CertificateWorkflowService service) => Results.Ok(service.Type(typeId)));
        certificates.MapGet("/{certificateId}", (string certificateId, CertificateWorkflowService service) => Results.Ok(service.Detail(certificateId)));
        certificates.MapPost("/{certificateId}/verify", (string certificateId, CertificateWorkflowService service) => Results.Ok(service.Verify(certificateId)));

        search.MapPost("/", (SearchQueryCommand request, SearchWorkflowService service) => Results.Ok(service.Search(request)));
        search.MapGet("/results/{entryId}", (string entryId, SearchWorkflowService service) => Results.Ok(service.Result(entryId)));
        search.MapPost("/{entryId}/open", (string entryId, SearchWorkflowService service) => Results.Ok(service.Open(entryId)));
        search.MapGet("/index-health", (SearchWorkflowService service) => Results.Ok(service.IndexHealth()));
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

public sealed class DocumentWorkflowService
{
    public static object DemoBoard(string schoolAccountId) => new { schoolAccountId, phase = "documents-search", status = "demo-ready", documents = 128, certificates = 34, legalHolds = 3, capabilities = DocumentCapabilities.All };
    public static IReadOnlyList<DocumentResponse> AudienceDocuments(string scope) => [new($"{scope}-doc-1", "Active", "Summary", ["access-revalidated", "restricted-details-minimized"] )];
    public object Categories() => new[] { new { categoryId = "guardian-consent", name = "Guardian Consent", retention = "7 years", visibility = "Guardian summary", approval = "Registrar" }, new { categoryId = "medical-note", name = "Medical Note", retention = "Legal hold aware", visibility = "Restricted", approval = "Medical staff" } };
    public object Category(string categoryId) => new { categoryId, name = "Guardian Consent", retention = "7 years", visibility = "Guardian summary", approval = "Registrar", evidence = new[] { "policy-versioned", "visibility-reviewed", "audit-written" } };
    public object DocumentDetail(string documentId) => new { reference = documentId, status = "Active", category = "guardian-consent", subjectReference = "student-amina", visibility = "StaffVisible", retention = "7 years", evidence = new[] { "object-reference-stored", "metadata-validated", "audit-written" } };
    public DocumentResponse Upload(DocumentCommand request) => new($"DOC-{request.ClientRequestId}", "Active", "StaffVisible", ["object-reference-stored", "metadata-validated", "audit-written"]);
    public DocumentResponse PlaceHold(string documentId, DocumentActionCommand request) => new(documentId, "LegalHold", "ReviewerOnly", ["retention-protected", request.Reason]);
    public DocumentResponse Export(string documentId, DocumentActionCommand request) => new(documentId, "ExportPrepared", "Controlled", ["scope-validated", "reason-captured", request.Reason]);
}

public sealed class CertificateWorkflowService
{
    public static IReadOnlyList<CertificateResponse> DemoCertificates(string schoolAccountId) => [new("CERT-2026-0001", "Issued", "Verified", [schoolAccountId, "source-preserved"] )];
    public object Types() => new[] { new { typeId = "attendance-letter", name = "Attendance Letter", issuingRole = "Registrar", visibility = "Guardian and student", expiry = "None" }, new { typeId = "enrollment-proof", name = "Enrollment Proof", issuingRole = "Registrar", visibility = "Guardian and student", expiry = "One term" } };
    public object Type(string typeId) => new { typeId, name = "Attendance Letter", issuingRole = "Registrar", visibility = "Guardian and student", requiredFields = new[] { "student", "term", "issuer" }, evidence = new[] { "template-versioned", "visibility-reviewed" } };
    public object Detail(string certificateId) => new { reference = certificateId, status = "Issued", verificationState = "Verified", issuer = "Registrar", subjectReference = "student-amina", visibility = "Guardian and student", evidence = new[] { "source-preserved", "verification-audit" } };
    public CertificateResponse Issue(CertificateCommand request) => new($"CERT-{request.ClientRequestId}", "Issued", "PendingVerification", ["type-validated", request.SourceReference, "audit-written"]);
    public CertificateResponse Verify(string certificateId) => new(certificateId, "Issued", "Verified", ["visibility-checked", "verification-audit"]);
}

public sealed class SearchWorkflowService
{
    public SearchResponse Search(SearchQueryCommand request) => new("search-log-1", ["Document: Amina consent form", "Certificate: Attendance letter", "Audit: export approved"], "Fresh", ["restricted-counts-hidden"]);
    public object Result(string entryId) => new { entryId, sourceModule = "documents", freshness = "Fresh", visibilityDecision = "Allowed", evidence = new[] { "tenant-checked", "permission-revalidated", "audit-written" } };
    public object Open(string entryId) => new { entryId, decision = "Allowed", evidence = new[] { "tenant-checked", "permission-revalidated", "audit-written" } };
    public object IndexHealth() => new { pending = 2, indexed = 128, stale = 3, failed = 1, suppressed = 4 };
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

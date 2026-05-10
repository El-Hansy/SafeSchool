using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;
using SafeSchool.Api.Features.IdentityAccess.Audit;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using SafeSchool.Api.Features.IdentityAccess.StudentProfiles;
using SafeSchool.Api.Infrastructure.Errors;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<IFeatureGateService, FeatureGateService>();
builder.Services.AddScoped<PermissionGuard>();
builder.Services.AddScoped<PermissionEvaluator>();
builder.Services.AddScoped<IAuditWriter, AuditWriter>();
builder.Services.AddScoped<IAccessDecisionWriter, AccessDecisionWriter>();
builder.Services.AddScoped<DuplicateStudentProfileDetector>();
builder.Services.AddScoped<StudentProfileAuditAdapter>();
builder.Services.AddScoped<StudentProfileService>();
builder.Services.AddScoped<RolePermissionService>();
builder.Services.AddScoped<GuardianAuditAdapter>();
builder.Services.AddScoped<GuardianService>();
builder.Services.AddScoped<GuardianLinkService>();
builder.Services.AddScoped<GuardianVisibilityService>();
builder.Services.AddScoped<CredentialAuditAdapter>();
builder.Services.AddScoped<CredentialLifecycleService>();
builder.Services.AddScoped<QrFallbackCredentialService>();
builder.Services.AddScoped<CredentialStatusSnapshotService>();
builder.Services.AddDbContext<SafeSchoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SafeSchool")));

var app = builder.Build();
app.UseMiddleware<ApiErrorMiddleware>();
app.UseMiddleware<TenantResolutionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityAccessEndpoints();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.Run();

public partial class Program;

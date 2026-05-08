using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SafeSchool.Api.Infrastructure.Persistence;

public sealed class SafeSchoolDbContextFactory : IDesignTimeDbContextFactory<SafeSchoolDbContext>
{
    public SafeSchoolDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseNpgsql("Host=localhost;Database=safeschool_design;Username=safeschool;Password=safeschool")
            .Options;
        return new SafeSchoolDbContext(options);
    }
}

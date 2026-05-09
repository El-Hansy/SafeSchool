using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Learning.Persistence;

namespace SafeSchool.Api.Features.Learning;

public static class LearningDbContextModelBuilderExtensions
{
    public static ModelBuilder ApplyLearningModel(this ModelBuilder modelBuilder) => modelBuilder.ApplyLearningEntityConfigurations();
}

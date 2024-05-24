using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tribe.Data.Configuration.Extensions;
using Tribe.Domain.Database.Configuration;
using Tribe.Domain.Models.Task;

namespace Tribe.Data.Configuration;

public class TaskConfiguration : BaseConfiguration<Domain.Models.Task.Task>
{
    public override void ConfigureChild(EntityTypeBuilder<Domain.Models.Task.Task> typeBuilder)
    {
        typeBuilder.Property(tc => tc.Content)
            .HasJsonConversion<TaskContent, TaskContent>();
    }
}
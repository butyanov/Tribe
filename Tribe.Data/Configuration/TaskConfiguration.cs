using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tribe.Data.Configuration.Extensions;
using Tribe.Domain.Database.Configuration;
using Tribe.Domain.Models.Task;
using Task = Tribe.Domain.Models.Task.Task;

namespace Tribe.Data.Configuration;

public class TaskConfiguration : BaseConfiguration<Task>
{
    public override void ConfigureChild(EntityTypeBuilder<Task> typeBuilder)
    {
        typeBuilder.Property(tc => tc.Content)
            .HasJsonConversion<TaskContent, TaskContent>();
    }
}
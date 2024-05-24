using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tribe.Data.Configuration.Extensions;
using Tribe.Domain.Database.Configuration;
using Tribe.Domain.Models.Tribe;

namespace Tribe.Data.Configuration;

public class TribeConfiguration : BaseConfiguration<Domain.Models.Tribe.Tribe>
{
    public override void ConfigureChild(EntityTypeBuilder<Domain.Models.Tribe.Tribe> typeBuilder)
    {
        typeBuilder.Property(tc => tc.Positions)
            .HasJsonConversion<IEnumerable<UserPosition>, List<UserPosition>>();
    }
}
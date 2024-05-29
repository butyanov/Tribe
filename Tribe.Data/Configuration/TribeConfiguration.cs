using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tribe.Data.Configuration.Extensions;
using Tribe.Domain.Database.Configuration;
using Tribe.Domain.Models.Tribe;
using tribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Data.Configuration;

public class TribeConfiguration : BaseConfiguration<tribeModel>
{
    public override void ConfigureChild(EntityTypeBuilder<tribeModel> typeBuilder)
    {
        typeBuilder.Property(tc => tc.Positions)
            .HasJsonConversion<IEnumerable<UserPosition>, List<UserPosition>>();

        typeBuilder
            .HasOne(t => t.Creator)
            .WithMany()
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        typeBuilder
            .HasMany(t => t.Participants)
            .WithMany(u => u.Tribes)
            .UsingEntity(j => j.ToTable("TribeParticipants")
            );
    }
}
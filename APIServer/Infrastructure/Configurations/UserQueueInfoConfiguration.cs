using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;
using Domain.Users.UserQueueInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserQueueInfoConfiguration : IEntityTypeConfiguration<UserQueueInfo>
{
    public void Configure(EntityTypeBuilder<UserQueueInfo> builder)
    {
        builder.HasKey(r => r.UserId);

        builder.Property(r => r.UserId).HasConversion(
            id => id.Id,
        id => new UserId(id));

        builder.Property(x => x.Status)
            .HasConversion(
                name => name.ToString(),
                name => (UserQueueStatus)Enum.Parse(typeof(UserQueueStatus), name)
                );

    }
}

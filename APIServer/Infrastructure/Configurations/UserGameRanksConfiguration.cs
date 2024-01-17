using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;
using Domain.Users.UserGamesRanks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserGameRanksConfiguration : IEntityTypeConfiguration<UserGameRank>
{
    private const string prefix2vs2 = "TwoVsTwo_";
    private const string prefix3vs3 = "ThreeVsThree";
    public void Configure(EntityTypeBuilder<UserGameRank> builder)
    {
        builder.HasKey(r => r.UserId);

        builder.Property(r => r.UserId).HasConversion(
            id => id.Id,
        id => new UserId(id));


        builder.OwnsOne(r => r.RocketLeague2vs2Rank,
            builder =>
            {
                builder.Property(x => x.RocketLeagueRankName).HasColumnName(prefix2vs2 + nameof(RocketLeagueRankName))
                .HasConversion(
                name => name.ToString(),
                name => (RocketLeagueRankName)Enum.Parse(typeof(RocketLeagueRankName), name)
                );

                builder.Property(x => x.RocketLeagueRankNumber).HasColumnName(prefix2vs2 + nameof(RocketLeagueRankNumber))
                    .HasConversion(
                    number => number.ToString(),
                    name => (RocketLeagueRankNumber)Enum.Parse(typeof(RocketLeagueRankNumber), name)
                    );

                builder.Property(x => x.RocketLeagueDivision).HasColumnName(prefix2vs2 + nameof(RocketLeagueDivision))
                    .HasConversion(
                    division => division.ToString(),
                    name => (RocketLeagueDivision)Enum.Parse(typeof(RocketLeagueDivision), name)
                    );
            });

        builder.OwnsOne(r => r.RocketLeague3vs3Rank,
            builder =>
            {
                builder.Property(x => x.RocketLeagueRankName).HasColumnName(prefix3vs3 + nameof(RocketLeagueRankName))
                .HasConversion(
                name => name.ToString(),
                name => (RocketLeagueRankName)Enum.Parse(typeof(RocketLeagueRankName), name)
                );

                builder.Property(x => x.RocketLeagueRankNumber).HasColumnName(prefix3vs3 + nameof(RocketLeagueRankNumber))
                    .HasConversion(
                    number => number.ToString(),
                    name => (RocketLeagueRankNumber)Enum.Parse(typeof(RocketLeagueRankNumber), name)
                    );

                builder.Property(x => x.RocketLeagueDivision).HasColumnName(prefix3vs3 + nameof(RocketLeagueDivision))
                    .HasConversion(
                    division => division.ToString(),
                    name => (RocketLeagueDivision)Enum.Parse(typeof(RocketLeagueDivision), name)
                    );
            });


    }
}
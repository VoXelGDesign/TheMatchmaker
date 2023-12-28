using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;

using Domain.Users.UserGamesRanks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Configurations;

internal class UserGameRanksConfiguration : IEntityTypeConfiguration<UserGameRank>
{
    public void Configure(EntityTypeBuilder<UserGameRank> builder)
    {
        builder.HasKey(r => r.UserId);

        builder.Property(r => r.UserId).HasConversion(
            id => id.Id,
        id => new UserId(id));

        builder.OwnsOne(r => r.RocketLeague1vs1Rank,
            builder =>
            {
                builder.Property(x => x.RocketLeagueRankName).HasColumnName(nameof(RocketLeagueRankName))
                .HasConversion(
                name => name.ToString(),
                name => (RocketLeagueRankName)Enum.Parse(typeof(RocketLeagueRankName), name)
                );

                builder.Property(x => x.RocketLeagueRankNumber).HasColumnName(nameof(RocketLeagueRankNumber))
                    .HasConversion(
                    number => number.ToString(),
                    name => (RocketLeagueRankNumber)Enum.Parse(typeof(RocketLeagueRankNumber), name)
                    );

                builder.Property(x => x.RocketLeagueDivision).HasColumnName(nameof(RocketLeagueDivision))
                    .HasConversion(
                    division => division.ToString(),
                    name => (RocketLeagueDivision)Enum.Parse(typeof(RocketLeagueDivision), name)
                    );
            });

        builder.OwnsOne(r => r.RocketLeague2vs2Rank,
            builder =>
            {
                builder.Property(x => x.RocketLeagueRankName).HasColumnName(nameof(RocketLeagueRankName))
                .HasConversion(
                name => name.ToString(),
                name => (RocketLeagueRankName)Enum.Parse(typeof(RocketLeagueRankName), name)
                );

                builder.Property(x => x.RocketLeagueRankNumber).HasColumnName(nameof(RocketLeagueRankNumber))
                    .HasConversion(
                    number => number.ToString(),
                    name => (RocketLeagueRankNumber)Enum.Parse(typeof(RocketLeagueRankNumber), name)
                    );

                builder.Property(x => x.RocketLeagueDivision).HasColumnName(nameof(RocketLeagueDivision))
                    .HasConversion(
                    division => division.ToString(),
                    name => (RocketLeagueDivision)Enum.Parse(typeof(RocketLeagueDivision), name)
                    );
            });

        builder.OwnsOne(r => r.RocketLeague3vs3Rank,
            builder =>
            {
                builder.Property(x => x.RocketLeagueRankName).HasColumnName(nameof(RocketLeagueRankName))
                .HasConversion(
                name => name.ToString(),
                name => (RocketLeagueRankName)Enum.Parse(typeof(RocketLeagueRankName), name)
                );

                builder.Property(x => x.RocketLeagueRankNumber).HasColumnName(nameof(RocketLeagueRankNumber))
                    .HasConversion(
                    number => number.ToString(),
                    name => (RocketLeagueRankNumber)Enum.Parse(typeof(RocketLeagueRankNumber), name)
                    );

                builder.Property(x => x.RocketLeagueDivision).HasColumnName(nameof(RocketLeagueDivision))
                    .HasConversion(
                    division => division.ToString(),
                    name => (RocketLeagueDivision)Enum.Parse(typeof(RocketLeagueDivision), name)
                    );
            });


    }
}
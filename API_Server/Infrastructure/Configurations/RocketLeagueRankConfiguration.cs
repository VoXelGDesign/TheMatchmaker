using Domain.Games.RocketLeague.Ranks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    internal class RocketLeagueRankConfiguration : IEntityTypeConfiguration<RocketLeagueRank>
    {
        public void Configure(EntityTypeBuilder<RocketLeagueRank> builder)
        {
            builder.Property(x => x.RocketLeagueRankName)
                .HasConversion(
                name => name.ToString(),
                name => (RocketLeagueRankName)Enum.Parse(typeof(RocketLeagueRankName), name)
                );

            builder.Property(x => x.RocketLeagueRankNumber)
                .HasConversion(
                number => number.ToString(),
                name => (RocketLeagueRankNumber)Enum.Parse(typeof(RocketLeagueRankNumber), name)
                );

            builder.Property(x => x.RocketLeagueDivision)
                .HasConversion(
                division => division.ToString(),
                name => (RocketLeagueDivision)Enum.Parse(typeof(RocketLeagueDivision), name)
                );
        }
    }
}

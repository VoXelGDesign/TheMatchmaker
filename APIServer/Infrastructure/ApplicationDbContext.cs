using Domain.Games.RocketLeague.Lobbies;
using Domain.Users.UserAccounts;
using Domain.Users.UserGamesRanks;
using Domain.Users.UserQueueInfos;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserGameRank> UserGameRanks { get; set; }
        public DbSet<UserQueueInfo> UserQueueInfos { get; set; }
        public DbSet<RocketLeague2vs2Lobby> RocketLeague2vs2Lobbies { get; set; }
        public DbSet<RocketLeague3vs3Lobby> RocketLeague3vs3Lobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserGameRanksConfiguration());
            modelBuilder.ApplyConfiguration(new UserQueueInfoConfiguration());
            modelBuilder.ApplyConfiguration(new RocketLeague2vs2LobbyConfiguration());
            modelBuilder.ApplyConfiguration(new RocketLeague3vs3LobbyConfiguration());
            base.OnModelCreating(modelBuilder);

        }
    }

}

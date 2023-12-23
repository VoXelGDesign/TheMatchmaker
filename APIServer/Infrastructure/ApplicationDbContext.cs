using Domain.Users.UserAccounts;
using Domain.Users.UserGamesRanks;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserGameRanksConfiguration());
            //modelBuilder.ApplyConfiguration(new RocketLeagueRankConfiguration());
            base.OnModelCreating(modelBuilder);


        }
    }

}

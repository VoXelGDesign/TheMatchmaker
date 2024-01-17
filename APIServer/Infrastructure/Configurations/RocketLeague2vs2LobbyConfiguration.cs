using Domain.Games.RocketLeague.Lobbies;
using Domain.Games.RocketLeague.Players;
using Domain.Users.User;
using Domain.Users.UserAccounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class RocketLeague2vs2LobbyConfiguration : IEntityTypeConfiguration<RocketLeague2vs2Lobby>
    {
        public void Configure(EntityTypeBuilder<RocketLeague2vs2Lobby> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasConversion(
            id => id.lobbyId,
            id => new RocketLeague2vs2LobbyId(id));

            builder.OwnsOne(x => x.Player1, playerBuilder =>
            {             
                playerBuilder.Property(u => u.UserId)
                .HasConversion(
                   id => id.Id,
                   id => new UserId(id))
                .HasColumnName("Player1_UserId");

                playerBuilder.Property(u => u.UserAccountName)
                .HasConversion(
                    name => name.Name,
                    name => UserAccountName.Create(name)!)
                .HasColumnName("Player1_Name");

                playerBuilder.Property(u => u.UserAccountSteamProfileLink)
                .HasConversion(
                        link => link.Link,
                        link => UserAccountSteamProfileLink.Create(link)!)
                .HasColumnName("Player1_SteamProfileLink");

                playerBuilder.Property(u => u.DiscordName)
                .HasConversion(
                    name => name.Name,
                    name => UserDiscordName.Create(name)!)
                .HasColumnName("Player1_DiscordName");

            });

            builder.OwnsOne(x => x.Player2, playerBuilder =>
            {

                playerBuilder.Property(u => u.UserId)
                .HasConversion(
                   id => id.Id,
                   id => new UserId(id))
                .HasColumnName("Player2_UserId");

                playerBuilder.Property(u => u.UserAccountName)
                .HasConversion(
                    name => name.Name,
                    name => UserAccountName.Create(name)!)
                .HasColumnName("Player2_Name");

                playerBuilder.Property(u => u.UserAccountSteamProfileLink)
                .HasConversion(
                        link => link.Link,
                        link => UserAccountSteamProfileLink.Create(link)!)
                .HasColumnName("Player2_SteamProfileLink");

                playerBuilder.Property(u => u.DiscordName)
                .HasConversion(
                    name => name.Name,
                    name => UserDiscordName.Create(name)!)
                .HasColumnName("Player2_DiscordName");

            });
        }
    }
}

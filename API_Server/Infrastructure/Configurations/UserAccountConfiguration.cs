using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users.UserAccounts;

namespace Infrastructure.Configurations;

internal class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasConversion(
            id => id.Id,
            id => new UserAccountId(id));

        builder.Property(u => u.Name).HasConversion(
            name => name.Name,
            name => UserAccountName.Create(name)!);

        builder.Property(u => u.SteamProfileLink).HasConversion(
            link => link.Link,
            link => UserAccountSteamProfileLink.Create(link)!);


    }
}


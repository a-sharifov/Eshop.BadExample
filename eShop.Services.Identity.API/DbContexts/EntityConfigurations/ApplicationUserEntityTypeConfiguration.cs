using eShop.Services.Identity.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Services.Identity.API.DbContexts.EntityConfigurations;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(pB => pB.FirstName)
               .HasMaxLength(64);

        builder.Property(pB => pB.LastName)
               .HasMaxLength(64);
    }
}
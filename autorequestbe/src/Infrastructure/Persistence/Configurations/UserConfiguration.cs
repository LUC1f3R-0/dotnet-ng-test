using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.UserUuid).HasDefaultValueSql("gen_random_uuid()").ValueGeneratedOnAdd().IsRequired();
        builder.HasIndex(u => u.UserUuid).IsUnique();

        builder.Property(u => u.Name).HasMaxLength(100);

        builder.Property(u => u.Email).HasMaxLength(150).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();

        builder.Property(u => u.Status).HasConversion<int>().IsRequired();

        builder.Property(u => u.IsVerified).IsRequired();

        builder.Property(u => u.Role).HasConversion<int>().IsRequired();

        builder.Property(u => u.CreatedAtUtc).IsRequired();

        builder.Property(u => u.UpdatedAtUtc).IsRequired();

        builder.Ignore(u => u.IsProfileComplete);
        builder.Ignore(u => u.CanAccessSystem);
    }
}
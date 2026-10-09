using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.SessionUuid).HasDefaultValueSql("gen_random_uuid()").ValueGeneratedOnAdd().IsRequired();
        builder.HasIndex(s => s.SessionUuid).IsUnique();

        builder.Property(s => s.UserId).IsRequired();
        builder.HasOne(s => s.User).WithMany(u => u.Sessions).HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.RefreshTokenHash).HasMaxLength(500).IsRequired();

        builder.Property(s => s.IpAddress).HasMaxLength(45);

        builder.Property(s => s.DeviceId).HasMaxLength(100);

        builder.Property(s => s.DeviceName).HasMaxLength(200);

        builder.Property(s => s.ExpiresAtUtc).IsRequired();
        
        builder.Property(s => s.RevokedAtUtc);

        builder.Property(s => s.CreatedAtUtc).IsRequired();

        builder.Property(s => s.UpdatedAtUtc).IsRequired();

    }
}
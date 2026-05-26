using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingSystem.Infrastructure.Configurations;

public class RateConfigurationConfiguration : IEntityTypeConfiguration<RateConfiguration>
{
    public void Configure(EntityTypeBuilder<RateConfiguration> builder)
    {
        builder.ToTable("rateconfigurations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PricePerMinute)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Active)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(
            new RateConfiguration
            {
                Id = 1,
                PricePerMinute = 50,
                Active = true
            }
        );
    }
}

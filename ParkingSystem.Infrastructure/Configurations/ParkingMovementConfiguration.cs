using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingSystem.Domain.Entities;

namespace ParkingSystem.Infrastructure.Configurations;

public class ParkingMovementConfiguration : IEntityTypeConfiguration<ParkingMovement>
{
    public void Configure(EntityTypeBuilder<ParkingMovement> builder)
    {
        builder.ToTable("parkingmovements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EntryDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.ExitDate)
            .HasColumnType("datetime");

        builder.Property(x => x.TotalMinutes);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(10, 2);

        builder.Property(x => x.Status)
           .HasConversion<string>()
           .HasMaxLength(20)
           .IsRequired();

        builder.Property(x => x.EmailSent)
            .HasDefaultValue(false);

        builder.Property(x => x.EntryDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.VehicleId);

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.ParkingMovements)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

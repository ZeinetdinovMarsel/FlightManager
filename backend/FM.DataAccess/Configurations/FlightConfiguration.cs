using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FM.DataAccess.Entities;
namespace FM.DataAccess.Configurations;
public partial class FlightConfiguration : IEntityTypeConfiguration<FlightEntity>
{
    public void Configure(EntityTypeBuilder<FlightEntity> builder)
    {
        builder.HasKey(f => f.Id);

        builder.HasOne(f => f.Airport)
               .WithMany(a => a.Flights)
               .HasForeignKey(f => f.AirportId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(f => f.FlightNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(f => f.Destination)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(f => f.DepartureTime)
               .IsRequired();

        builder.Property(f => f.ArrivalTime)
               .IsRequired();

        builder.Property(f => f.AvailableSeats)
               .IsRequired();

        builder.Property(f => f.AirplanePhotoUrl)
               .HasMaxLength(500);

        builder.HasData(SeedData.Flights);
    }
}
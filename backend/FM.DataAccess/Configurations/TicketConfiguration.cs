using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace FM.DataAccess.Configurations;
public partial class TicketConfiguration : IEntityTypeConfiguration<TicketEntity>
{
    public void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.Flight)
               .WithMany(f => f.Tickets)
               .HasForeignKey(t => t.FlightId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.Property(t => t.Seat)
               .IsRequired()
               .HasMaxLength(10);
        builder.Property(t => t.Price)
               .IsRequired();

        builder.HasData(SeedData.Tickets);
    }
}
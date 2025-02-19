using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FM.DataAccess.Entities;

namespace FM.DataAccess.Configurations;
public partial class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.Cost)
               .IsRequired();

        builder.HasMany(s => s.TicketService)
               .WithOne(ts => ts.Service)
               .HasForeignKey(ts => ts.ServiceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

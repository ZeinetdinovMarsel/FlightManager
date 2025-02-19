using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FM.DataAccess.Entities;

namespace FM.DataAccess.Configurations;
public partial class TicketServiceConfiguration : IEntityTypeConfiguration<TicketServiceEntity>
{
    public void Configure(EntityTypeBuilder<TicketServiceEntity> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.HasOne(ts => ts.Ticket)
               .WithMany(t => t.Services)
               .HasForeignKey(ts => ts.TicketId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ts => ts.Service)
               .WithMany(s => s.TicketService)
               .HasForeignKey(ts => ts.ServiceId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ts => ts.ServiceId)
               .IsRequired();

        builder.Property(ts => ts.TicketId)
               .IsRequired();
    }
}

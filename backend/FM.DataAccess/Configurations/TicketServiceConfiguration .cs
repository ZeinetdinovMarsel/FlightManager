using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace FM.DataAccess.Configurations;
public partial class TicketServiceConfiguration : IEntityTypeConfiguration<TicketServiceEntity>
{
    public void Configure(EntityTypeBuilder<TicketServiceEntity> builder)
    {
        builder.HasKey(ts => ts.Id);
        builder.Property(ts => ts.ServiceName)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(ts => ts.ServiceCost)
               .IsRequired();
    }
}
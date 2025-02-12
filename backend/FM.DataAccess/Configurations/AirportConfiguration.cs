using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace FM.DataAccess.Configurations;
public partial class AirportConfiguration : IEntityTypeConfiguration<AirportEntity>
{
    public void Configure(EntityTypeBuilder<AirportEntity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.FederalDistrict)
               .WithMany(f => f.Airports)
               .HasForeignKey(a => a.FederalDistrictId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.City)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.FederalDistrictId)
               .IsRequired();

    }
}
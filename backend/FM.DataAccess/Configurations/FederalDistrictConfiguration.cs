using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FM.DataAccess.Entities;

namespace FM.DataAccess.Configurations;

public class FederalDistrictConfiguration : IEntityTypeConfiguration<FederalDistrictEntity>
{
    public void Configure(EntityTypeBuilder<FederalDistrictEntity> builder)
    {

        builder.HasKey(fd => fd.Id);

        builder.Property(fd => fd.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.Property(fd => fd.Name)
               .IsRequired()
               .HasMaxLength(255);


        builder.HasMany(fd => fd.Airports)
               .WithOne(a => a.FederalDistrict)
               .HasForeignKey(a => a.FederalDistrictId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(SeedData.FederalDistricts);
    }
}
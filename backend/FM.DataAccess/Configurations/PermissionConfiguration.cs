using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FM.DataAccess.Entities;
using FM.Core.Enums;

namespace FM.DataAccess.Configurations;
public partial class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{


    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasKey(p => p.Id);

        var permissions = Enum
            .GetValues<Permission>()
            .Select(p => new PermissionEntity
            {
                Id = (int)p,
                Name = p.ToString()
            });
        builder.HasData(permissions);
    }
}

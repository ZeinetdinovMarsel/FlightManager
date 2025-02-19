using Microsoft.EntityFrameworkCore;
using FM.DataAccess.Entities;
using Microsoft.Extensions.Options;
using FM.DataAccess.Configurations;
namespace FM.DataAccess;
public class FMDbContext(
    DbContextOptions<FMDbContext> options,
    IOptions<AuthorizationOptions> authOptions) : DbContext(options)
{
    public DbSet<FederalDistrictEntity> FederalDistricts { get; set; }
    public DbSet<AirportEntity> Airports { get; set; }
    public DbSet<FlightEntity> Flights { get; set; }
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<TicketServiceEntity> TicketServices { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserRoleEntity> UserRoleEntity { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FMDbContext).Assembly);
        
       

        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceConfiguration());
        modelBuilder.ApplyConfiguration(new TicketServiceConfiguration());
        modelBuilder.ApplyConfiguration(new FlightConfiguration());
        modelBuilder.ApplyConfiguration(new AirportConfiguration());
    }
}

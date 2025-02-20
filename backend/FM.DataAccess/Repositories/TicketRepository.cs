using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
namespace FM.DataAccess.Repositories;
public class TicketRepository : ITicketRepository
{
    private readonly FMDbContext _context;

    public TicketRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketModel>> GetAllAsync(
    string? sortBy = null,
    bool descending = false,
    int page = 1,
    int pageSize = 10,
    int? ticketTypeFilter = null,
    float? priceFilter = null,
    string? seatFilter = null,
    int? flightIdFilter = null,
    int[]? serviceIdsFilter = null
)
    {
        var query = _context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Services)
            .AsQueryable();

        if (ticketTypeFilter.HasValue)
        {
            query = query.Where(t => t.TicketType == (Core.Enums.TicketType)ticketTypeFilter.Value);
        }

        if (priceFilter.HasValue)
        {
            query = query.Where(t => t.Price == priceFilter.Value);
        }

        if (!string.IsNullOrWhiteSpace(seatFilter))
        {
            query = query.Where(t => t.Seat.Contains(seatFilter));
        }

        if (flightIdFilter.HasValue)
        {
            query = query.Where(t => t.FlightId == flightIdFilter.Value);
        }

        if (serviceIdsFilter != null && serviceIdsFilter.Any())
        {
            foreach (var serviceId in serviceIdsFilter)
            {
                query = query.Where(t => t.Services.Any(s => s.ServiceId == serviceId));
            }
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "id" => descending ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id),
                "tickettype" => descending ? query.OrderByDescending(t => t.TicketType) : query.OrderBy(t => t.TicketType),
                "price" => descending ? query.OrderByDescending(t => t.Price) : query.OrderBy(t => t.Price),
                "seat" => descending ? query.OrderByDescending(t => t.Seat) : query.OrderBy(t => t.Seat),
                "flight" => descending ? query.OrderByDescending(t => t.Flight.FlightNumber) : query.OrderBy(t => t.Flight.FlightNumber),
                "flightid" => descending ? query.OrderByDescending(t => t.FlightId) : query.OrderBy(t => t.FlightId),
                "services" => descending
                    ? query.OrderByDescending(t => t.Services.Count)
                    : query.OrderBy(t => t.Services.Count),
                _ => query
            };
        }

        var skip = (page - 1) * pageSize;
        var tickets = await query.Skip(skip).Take(pageSize).ToListAsync();

        return tickets.Select(t => TicketModel.Create(
            t.Id,
            t.TicketType,
            t.Price,
            t.Seat,
            t.FlightId,
            t.Services.Select(s => TicketServiceModel.Create(s.Id, s.ServiceId, t.Id)).ToList()
        ));
    }


    public async Task<TicketModel?> GetByIdAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Services)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return null;
        }

        return TicketModel.Create(
            ticket.Id,
            ticket.TicketType,
            ticket.Price,
            ticket.Seat,
            ticket.FlightId,
            ticket.Services.Select(s => TicketServiceModel.Create(s.Id, s.ServiceId, ticket.Id)).ToList()
        );
    }

    public async Task<int> CreateAsync(TicketModel ticketModel)
    {
        var ticketEntity = new TicketEntity
        {
            TicketType = ticketModel.TicketType,
            Price = ticketModel.Price,
            Seat = ticketModel.Seat,
            FlightId = ticketModel.FlightId

        };

        _context.Tickets.Add(ticketEntity);

        await _context.SaveChangesAsync();

        foreach (var service in ticketModel.Services)
        {
            var serviceEntity = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == service.ServiceId);

            var ticketServiceEntity = new TicketServiceEntity
            {
                TicketId = ticketEntity.Id,
                Ticket = ticketEntity,
                ServiceId = service.ServiceId,
                Service = serviceEntity
            };

            ticketEntity.Services.Add(ticketServiceEntity);
            _context.TicketServices.Add(ticketServiceEntity);
        }
        

        var ticket = await _context.Tickets
           .FirstOrDefaultAsync(t => t.Id == ticketEntity.Id);

        ticket.Services = ticketEntity.Services;

        await _context.SaveChangesAsync();

        return ticketEntity.Id;
    }

    public async Task<bool> UpdateAsync(int id, TicketModel ticketModel)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            throw new Exception("Билет не найден");
        }

        ticket.TicketType = ticketModel.TicketType;
        ticket.Price = ticketModel.Price;
        ticket.Seat = ticketModel.Seat;
        ticket.FlightId = ticketModel.FlightId;

        var existingServices = _context.TicketServices
            .Where(s => s.TicketId == id)
            .ToList();

        _context.TicketServices.RemoveRange(existingServices);

        foreach (var service in ticketModel.Services)
        {
            var serviceEntity = await _context.Services
            .FirstOrDefaultAsync(t => t.Id == service.ServiceId);

            var ticketServiceEntity = new TicketServiceEntity
            {
                TicketId = ticket.Id,
                Ticket = ticket,
                ServiceId = service.ServiceId,
                Service = serviceEntity
            };

            _context.TicketServices.Add(ticketServiceEntity);
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return false;
        }

        _context.Tickets.Remove(ticket);

        var services = _context.TicketServices
            .Where(s => s.TicketId == id)
            .ToList();

        _context.TicketServices.RemoveRange(services);

        await _context.SaveChangesAsync();

        return true;
    }
}

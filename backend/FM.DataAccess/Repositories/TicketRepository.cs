using Microsoft.EntityFrameworkCore;
namespace FM.DataAccess.Repositories;
public class TicketRepository : ITicketRepository
{
    private readonly FMDbContext _context;

    public TicketRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        var query = _context.Tickets.Include(t => t.Flight).Include(t => t.Services).AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(t => t.Seat.Contains(filter) || t.Flight.FlightNumber.Contains(filter));
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "tickettype" => descending ? query.OrderByDescending(t => t.TicketType) : query.OrderBy(t => t.TicketType),
                "price" => descending ? query.OrderByDescending(t => t.Price) : query.OrderBy(t => t.Price),
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
            t.Services.Select(s => TicketServiceModel.Create(s.Id, s.ServiceName, s.ServiceCost)).ToList()
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
            ticket.Services.Select(s => TicketServiceModel.Create(s.Id, s.ServiceName, s.ServiceCost)).ToList()
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
            var serviceEntity = new TicketServiceEntity
            {
                TicketId = ticketEntity.Id,
                ServiceName = service.ServiceName,
                ServiceCost = service.ServiceCost
            };

            _context.TicketServices.Add(serviceEntity);
        }

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
            var serviceEntity = new TicketServiceEntity
            {
                TicketId = ticket.Id,
                ServiceName = service.ServiceName,
                ServiceCost = service.ServiceCost
            };

            _context.TicketServices.Add(serviceEntity);
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

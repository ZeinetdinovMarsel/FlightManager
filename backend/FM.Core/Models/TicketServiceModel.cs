public class TicketServiceModel
{
    private TicketServiceModel(int id, string serviceName, float serviceCost)
    {
        Id = id;
        ServiceName = serviceName;
        ServiceCost = serviceCost;
    }
    public int Id { get; private set; }
    public string ServiceName { get; private set; } = string.Empty;
    public float ServiceCost { get; private set; }
    public static TicketServiceModel Create(int id, string serviceName, float serviceCost)
    {
        return new TicketServiceModel(id, serviceName, serviceCost);
    }
}

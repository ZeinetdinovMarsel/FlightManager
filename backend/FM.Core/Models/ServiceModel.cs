using System;

namespace FM.Core.Models;
public class ServiceModel
{
    private ServiceModel(int id, string name, decimal cost)
    {
        Id = id;
        Name = name;
        Cost = cost;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Cost { get; private set; }

    public static ServiceModel Create(int id, string name, decimal cost)
    {
        return new ServiceModel(id, name, cost);
    }
}
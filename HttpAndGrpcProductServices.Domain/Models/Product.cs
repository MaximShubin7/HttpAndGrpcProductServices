using HttpAndGrpcProductServices.Domain.Enums;

namespace HttpAndGrpcProductServices.Domain.Models;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public ProductType Type { get; set; }
    public DateTime CreatedAt { get; init; }
    public int WarehouseNumber { get; set; }
}
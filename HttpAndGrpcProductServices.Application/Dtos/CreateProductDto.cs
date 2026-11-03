namespace HttpAndGrpcProductServices.Application.Dtos;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public int Type { get; set; }
    public int WarehouseNumber { get; set; }
}
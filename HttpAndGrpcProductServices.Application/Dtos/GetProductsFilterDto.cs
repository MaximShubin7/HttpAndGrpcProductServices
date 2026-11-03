namespace HttpAndGrpcProductServices.Application.Dtos;

public class GetProductsFilterDto
{
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public int? Type { get; set; }
    public int? WarehouseNumber { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
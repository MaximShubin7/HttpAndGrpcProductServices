using HttpAndGrpcProductServices.Application.Dtos;

namespace HttpAndGrpcProductServices.Application.Abstractions;

public interface IProductService
{
    Task<Guid> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<ProductDto> Items, int TotalCount)> GetFilteredAsync(
        GetProductsFilterDto filter, CancellationToken ct = default);
}
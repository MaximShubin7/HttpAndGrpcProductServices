using HttpAndGrpcProductServices.Domain.Abstractions;
using HttpAndGrpcProductServices.Domain.Enums;
using HttpAndGrpcProductServices.Domain.Models;
using System.Collections.Concurrent;

namespace HttpAndGrpcProductServices.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Task<Guid> AddAsync(Product product, CancellationToken ct = default)
    {
        _products[product.Id] = product;
        return Task.FromResult(product.Id);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<(IReadOnlyList<Product> Items, int TotalCount)> GetFilteredAsync(
        DateTime? createdFrom,
        DateTime? createdTo,
        ProductType? type,
        int? warehouseNumber,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _products.Values.AsEnumerable();

        if (createdFrom.HasValue)
            query = query.Where(p => p.CreatedAt >= createdFrom.Value);

        if (createdTo.HasValue)
            query = query.Where(p => p.CreatedAt <= createdTo.Value);

        if (type.HasValue)
            query = query.Where(p => p.Type == type.Value);

        if (warehouseNumber.HasValue)
            query = query.Where(p => p.WarehouseNumber == warehouseNumber.Value);

        var list = query.OrderByDescending(p => p.CreatedAt).ToList();
        var total = list.Count;
        var items = list
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult<(IReadOnlyList<Product>, int)>((items, total));
    }
}
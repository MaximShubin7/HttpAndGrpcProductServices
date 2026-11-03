using FluentValidation;
using Grpc.Core;
using HttpAndGrpcProductServices.Application.Abstractions;
using HttpAndGrpcProductServices.Application.Dtos;
using static HttpAndGrpcProductServices.Grpc.ProductService;

namespace HttpAndGrpcProductServices.Grpc.Services;

public class ProductGrpcService : ProductServiceBase
{
    private readonly IProductService _service;
    private readonly IValidator<CreateProductDto> _createValidator;

    public ProductGrpcService(
        IProductService service,
        IValidator<CreateProductDto> createValidator)
    {
        _service = service;
        _createValidator = createValidator;
    }

    public override async Task<CreateProductResponse> CreateProduct(
        CreateProductRequest request,
        ServerCallContext context)
    {
        var dto = new CreateProductDto
        {
            Name = request.Name,
            Price = (decimal)request.Price,
            Weight = request.Weight,
            Type = request.Type,
            WarehouseNumber = request.WarehouseNumber
        };

        var validation = await _createValidator.ValidateAsync(dto, context.CancellationToken);
        if (!validation.IsValid)
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var id = await _service.CreateAsync(dto, context.CancellationToken);
        return new CreateProductResponse { Id = id.ToString() };
    }

    public override async Task<GetProductsResponse> GetProducts(
        GetProductsRequest request,
        ServerCallContext context)
    {
        DateTime? from = DateTime.TryParse(request.CreatedFrom, out var f) ? f : null;
        DateTime? to = DateTime.TryParse(request.CreatedTo, out var t) ? t : null;

        var filter = new GetProductsFilterDto
        {
            CreatedFrom = from,
            CreatedTo = to,
            Type = request.HasType ? request.Type : null,
            WarehouseNumber = request.HasWarehouseNumber ? request.WarehouseNumber : null,
            PageNumber = request.PageNumber > 0 ? request.PageNumber : 1,
            PageSize = request.PageSize > 0 ? request.PageSize : 10
        };

        var (items, total) = await _service.GetFilteredAsync(filter, context.CancellationToken);

        var response = new GetProductsResponse { TotalCount = total };
        response.Items.AddRange(items.Select(p => new ProductMessage
        {
            Id = p.Id.ToString(),
            Name = p.Name,
            Price = (double)p.Price,
            Weight = p.Weight,
            Type = p.Type,
            CreatedAt = p.CreatedAt.ToString("O"),
            WarehouseNumber = p.WarehouseNumber
        }));
        return response;
    }

    public override async Task<GetProductByIdResponse> GetProductById(
        GetProductByIdRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный Id"));

        var product = await _service.GetByIdAsync(id, context.CancellationToken);
        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Товар с Id '{id}' не найден"));

        return new GetProductByIdResponse
        {
            Product = new ProductMessage
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = (double)product.Price,
                Weight = product.Weight,
                Type = product.Type,
                CreatedAt = product.CreatedAt.ToString("O"),
                WarehouseNumber = product.WarehouseNumber
            }
        };
    }
}
using FluentValidation;
using HttpAndGrpcProductServices.Application.Abstractions;
using HttpAndGrpcProductServices.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpAndGrpcProductServices.Http.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductDto dto,
        [FromServices] IValidator<CreateProductDto> validator,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFiltered(
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo,
        [FromQuery] int? type,
        [FromQuery] int? warehouseNumber,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var filter = new GetProductsFilterDto
        {
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            Type = type,
            WarehouseNumber = warehouseNumber,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (items, total) = await _service.GetFilteredAsync(filter, ct);
        return Ok(new { items, totalCount = total, pageNumber, pageSize });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await _service.GetByIdAsync(id, ct);
        if (product is null)
            return NotFound(new { message = $"Товар с Id '{id}' не найден" });

        return Ok(product);
    }
}
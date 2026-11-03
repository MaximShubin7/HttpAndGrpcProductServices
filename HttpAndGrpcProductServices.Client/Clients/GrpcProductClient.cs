using Grpc.Core;
using Grpc.Net.Client;
using HttpAndGrpcProductServices.Client.Abstractions;
using HttpAndGrpcProductServices.Grpc;
using static HttpAndGrpcProductServices.Grpc.ProductService;

namespace HttpAndGrpcProductServices.Client.Clients;

public sealed class GrpcProductClient : IProductClient, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly ProductServiceClient _client;

    public GrpcProductClient(string baseUrl)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        _channel = GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions { HttpHandler = handler });
        _client = new ProductServiceClient(_channel);
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        Console.WriteLine("\ngRPC клиент");

        var created = await _client.CreateProductAsync(new CreateProductRequest
        {
            Name = "Ноутбук",
            Price = 75000.50,
            Weight = 2.1,
            Type = 2,
            WarehouseNumber = 1
        }, cancellationToken: ct);
        Console.WriteLine($"Создан: {created.Id}");

        var product = await _client.GetProductByIdAsync(new GetProductByIdRequest { Id = created.Id }, cancellationToken: ct);
        Console.WriteLine($"Найден: {product.Product.Name} ({product.Product.Price})");

        try
        {
            await _client.GetProductByIdAsync(new GetProductByIdRequest { Id = Guid.NewGuid().ToString() }, cancellationToken: ct);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            Console.WriteLine("Поиск несуществующего ID: 404");
        }

        var list = await _client.GetProductsAsync(new GetProductsRequest { PageNumber = 1, PageSize = 5 }, cancellationToken: ct);
        Console.WriteLine($"Всего товаров: {list.TotalCount}");
    }

    public void Dispose() => _channel.Dispose();
}
using HttpAndGrpcProductServices.Client.Abstractions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HttpAndGrpcProductServices.Client.Clients;

public sealed class HttpProductClient : IProductClient, IDisposable
{
    private readonly HttpClient _http;

    public HttpProductClient(string baseUrl)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        _http = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        Console.WriteLine("\nHTTP клиент");

        var createResponse = await _http.PostAsJsonAsync("/api/products", new
        {
            name = "Порошок",
            price = 450.00m,
            weight = 3.0,
            type = 1,
            warehouseNumber = 2
        }, ct);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>(ct);
        var id = created.GetProperty("id").GetGuid();
        Console.WriteLine($"Создан: {id}");

        var getResponse = await _http.GetAsync($"/api/products/{id}", ct);
        getResponse.EnsureSuccessStatusCode();
        var product = await getResponse.Content.ReadFromJsonAsync<JsonElement>(ct);
        Console.WriteLine($"Найден: {product.GetProperty("name").GetString()} ({product.GetProperty("price").GetDecimal()})");

        var notFound = await _http.GetAsync($"/api/products/{Guid.NewGuid()}", ct);
        if (notFound.StatusCode == HttpStatusCode.NotFound)
            Console.WriteLine("Поиск несуществующего ID: 404");

        var listResponse = await _http.GetAsync("/api/products?pageSize=5", ct);
        listResponse.EnsureSuccessStatusCode();
        var list = await listResponse.Content.ReadFromJsonAsync<JsonElement>(ct);
        Console.WriteLine($"Всего товаров: {list.GetProperty("totalCount").GetInt32()}");
    }

    public void Dispose() => _http.Dispose();
}
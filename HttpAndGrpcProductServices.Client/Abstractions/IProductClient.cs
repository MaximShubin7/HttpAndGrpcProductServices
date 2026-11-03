namespace HttpAndGrpcProductServices.Client.Abstractions;

public interface IProductClient
{
    Task RunAsync(CancellationToken ct = default);
}
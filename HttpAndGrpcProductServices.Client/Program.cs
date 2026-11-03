using HttpAndGrpcProductServices.Client.Clients;

namespace HttpAndGrpcProductServices.Client;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        var baseUrl = args.Length > 0 ? args[0] : "https://localhost:7071";

        try
        {
            using var grpc = new GrpcProductClient(baseUrl);
            await grpc.RunAsync();

            using var http = new HttpProductClient(baseUrl);
            await http.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.ReadLine();

        return 0;
    }
}
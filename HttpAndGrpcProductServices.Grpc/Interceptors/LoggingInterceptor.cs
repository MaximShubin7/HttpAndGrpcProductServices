using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace HttpAndGrpcProductServices.Grpc.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("gRPC запрос: {Method}, Request: {@Request}",
            context.Method, request);

        try
        {
            var response = await continuation(request, context);
            _logger.LogInformation("gRPC ответ: {Method}, Response: {@Response}",
                context.Method, response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC ошибка: {Method}", context.Method);
            throw;
        }
    }
}
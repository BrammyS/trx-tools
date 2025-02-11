using Microsoft.Extensions.DependencyInjection;

namespace trx_tools.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}
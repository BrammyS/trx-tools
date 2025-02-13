using Microsoft.Extensions.DependencyInjection;
using trx_tools.Commands.Interfaces;

namespace trx_tools.Commands;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandHandler(this IServiceCollection services)
    {
        services.AddSingleton<ICommandHandler, CommandHandler>();
        return services;
    }
}
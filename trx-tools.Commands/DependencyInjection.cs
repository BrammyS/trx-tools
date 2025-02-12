using Microsoft.Extensions.DependencyInjection;
using trx_tools.Commands.Abstraction.Interfaces;

namespace trx_tools.Commands.Abstraction;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandHandler(this IServiceCollection services)
    {
        services.AddSingleton<ICommandHandler, CommandHandler>();
        return services;
    }
}
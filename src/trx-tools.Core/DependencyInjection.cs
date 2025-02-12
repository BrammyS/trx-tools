using Microsoft.Extensions.DependencyInjection;
using trx_tools.Core.Services;
using trx_tools.Core.Services.Interfaces;

namespace trx_tools.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<ITestRunTrxFileService, TestRunTrxFileService>();
        services.AddSingleton<ITestRunParserService, TestRunParserService>();
        services.AddSingleton<ITestRunMergerService, TestRunMergerService>();
        
        return services;
    }
}
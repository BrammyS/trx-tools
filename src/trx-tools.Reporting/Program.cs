using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using trx_tools.Commands.Abstraction;
using trx_tools.Commands.Abstraction.Interfaces;
using trx_tools.Core;
using trx_tools.HtmlReporting.Commands;
using trx_tools.HtmlReporting.Services;
using trx_tools.HtmlReporting.Services.Interfaces;

var serviceCollection = new ServiceCollection()
    .AddLogging(builder => builder.AddConsole())
    .AddCore()
    .AddCommandHandler()
    .AddTransient<IHtmlReportingService, HtmlReportingService>()
    .Scan(selector => selector
        .FromAssemblyOf<HtmlCommand>()
        .AddClasses(classes => classes.AssignableTo<ICommand>())
        .AsImplementedInterfaces()
        .WithTransientLifetime()
    );

var serviceProvider = serviceCollection.BuildServiceProvider();
var commandHandler = serviceProvider.GetRequiredService<ICommandHandler>();

await commandHandler.HandleCommandAsync(args);

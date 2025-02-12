using Microsoft.Extensions.Logging;
using trx_tools.Commands.Abstraction;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Commands;

public class HtmlCommand(ILogger<HtmlCommand> logger, IHtmlReportingService htmlReportingService) : ICommand
{
    public string Name => "html";
    public string Description => "Generate HTML report from TRX file(s). Example: trx-tools.Reporting html path/to/trx/directory output.html";

    public async Task ExecuteAsync(string[] args)
    {
        if (!ValidateInput(args)) return;
        await htmlReportingService.GenerateHtmlReportAsync(args[0], args[1]);
    }

    private bool ValidateInput(string[] args)
    {
        if (args.Length != 2)
        {
            logger.LogError("Invalid number of arguments provided, see help command for example");
            return false;
        }
        
        if (!args[1].EndsWith(".html"))
        {
            logger.LogError("Output file must be an HTML file");
            return false;
        }

        return true;
    }
}
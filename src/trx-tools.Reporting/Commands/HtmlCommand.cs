using Microsoft.Extensions.Logging;
using trx_tools.Commands;
using trx_tools.Core.Services.Interfaces;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Commands;

public class HtmlCommand(ILogger<HtmlCommand> logger, IHtmlReportingService htmlReportingService, ITestRunTrxFileService fileService) : ICommand
{
    public string Name => "html";
    public string Description => "Generate HTML report from TRX file(s). Example: trx-tools.Reporting html path/to/trx/directory output.html [--only-latest] [--only-files file1.trx file2.trx]";

    public async Task ExecuteAsync(CLIArgHandler args)
    {
        var unamedArgs = args.GetInitialUnnamedArgs();
        if (!ValidateInput(unamedArgs))
            return;

        var trxDirectory = unamedArgs[0];
        var outputFile = unamedArgs[1];

        var onlyFiles = args.GetOptArr("only-files");
        if (onlyFiles?.Length > 0)
        {
            ResolveFilePaths(trxDirectory, onlyFiles);
            var missingFiles = onlyFiles.Where(f => !fileService.FileExists(f)).ToList();
            if (missingFiles.Any())
                throw new FileNotFoundException($"The following files specified in --only-files could not be resolved: {string.Join(", ", missingFiles)}");
        }

        await htmlReportingService.GenerateHtmlReportAsync(trxDirectory, outputFile, new() { latestTrxOnly = args.GetFlag("only-latest"), onlyFiles = onlyFiles });
    }

    private void ResolveFilePaths(string trxDirectory, string[] onlyFiles)
    {
        for (var i = 0; i < onlyFiles.Length; i++)
        {
            var filePath = onlyFiles[i];

            // If path is rooted, use it as-is
            if (Path.IsPathRooted(filePath))
                continue;
            else
            {
                // Try combining with trx directory first
                var combinedPath = Path.Combine(Directory.GetCurrentDirectory(), trxDirectory, filePath);
                if (fileService.FileExists(combinedPath))
                    onlyFiles[i] = combinedPath;
                else                    // Fall back to working directory

                    onlyFiles[i] = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            }
        }
    }



    private bool ValidateInput(string[] unamedArgs)
    {

        if (unamedArgs.Length != 2)
        {
            logger.LogError("Invalid number of arguments provided, see help command for example");
            return false;
        }

        if (!unamedArgs[1].EndsWith(".html"))
        {
            logger.LogError("Output file must be an HTML file");
            return false;
        }

        return true;
    }
}

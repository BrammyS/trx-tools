using Microsoft.Extensions.Logging;
using trx_tools.Commands;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Commands;

public class HtmlCommand(ILogger<HtmlCommand> logger, IHtmlReportingService htmlReportingService) : ICommand
{
    public string Name => "html";
    public string Description => "Generate HTML report from TRX file(s). Example: trx-tools.Reporting html path/to/trx/directory output.html [--include-output] [--pretty] [--only-latest] [--only-files file1.trx file2.trx]";

    public async Task ExecuteAsync(string[] args)
    {
        if (!ValidateInput(args)) return;

        var trxDirectory = args[0];
        var outputFile = args[1];
        var latestOnly = args.Contains("--only-latest");
        var includeOutput = args.Contains("--include-output");
        var pretty = args.Contains("--pretty");
        var onlyFiles = ParseOnlyFiles(args, trxDirectory);

        await htmlReportingService.GenerateHtmlReportAsync(trxDirectory, outputFile, latestOnly, onlyFiles, includeOutput, pretty);
    }

    private static List<string>? ParseOnlyFiles(string[] args, string trxDirectory)
    {
        var onlyFiles = new List<string>();
        
        for (var i = 2; i < args.Length; i++)
        {
            if (args[i] == "--only-files")
            {
                // Read all files until we hit another -- argument or end of args
                for (var j = i + 1; j < args.Length && !args[j].StartsWith("--"); j++)
                {
                    var filePath = args[j];
                    
                    // If path is rooted, use it as-is
                    if (Path.IsPathRooted(filePath))
                    {
                        onlyFiles.Add(filePath);
                    }
                    else
                    {
                        // Try combining with trx directory first
                        var combinedPath = Path.Combine(Directory.GetCurrentDirectory(), trxDirectory, filePath);
                        if (File.Exists(combinedPath))
                        {
                            onlyFiles.Add(combinedPath);
                        }
                        else
                        {
                            // Fall back to working directory
                            onlyFiles.Add(Path.Combine(Directory.GetCurrentDirectory(), filePath));
                        }
                    }
                }
                break;
            }
        }

        return onlyFiles.Count > 0 ? onlyFiles : null;
    }

    private bool ValidateInput(string[] args)
    {
        if (args.Length < 2)
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
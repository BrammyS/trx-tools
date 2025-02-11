using trx_tools.Core.Models;
using trx_tools.Core.Models.Parsed;

namespace trx_tools.Core.Services.Interfaces;

public interface ITestRunParserService
{
     ParsedTestRun ParseTestRun(TestRun testRun);
}
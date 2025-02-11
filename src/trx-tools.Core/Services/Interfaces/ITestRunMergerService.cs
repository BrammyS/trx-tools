using trx_tools.Core.Models;

namespace trx_tools.Core.Services.Interfaces;

public interface ITestRunMergerService
{
    Task<TestRun> MergeTestListsAsync(IEnumerable<TestRun> testRuns);
}
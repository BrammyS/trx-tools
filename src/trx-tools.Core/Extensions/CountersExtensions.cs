using trx_tools.Core.Models.ResultSummary;

namespace trx_tools.Core.Extensions;

/// <summary>
///     The extension methods for the <see cref="Counters" /> class.
/// </summary>
public static class CountersExtensions
{
    /// <summary>
    ///     Get the number of tests that were skipped.
    /// </summary>
    /// <remarks>
    ///     This method sadly exists because there seems to be a bug in the counters provided by the trx files.
    /// </remarks>
    /// <param name="counters">The counters to get the skipped test count from.</param>
    /// <returns>
    ///     The number of tests that were skipped.
    /// </returns>
    public static uint GetSkippedTestCount(this Counters counters)
    {
        return counters.Total - counters.Executed;
    }
}
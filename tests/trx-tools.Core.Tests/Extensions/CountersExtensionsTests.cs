using FluentAssertions;
using NUnit.Framework;
using trx_tools.Core.Extensions;
using trx_tools.Core.Models.ResultSummary;

namespace trx_tools.Core.Tests.Extensions;

[TestFixture]
public class CountersExtensionsTests
{
    [Test]
    [TestCase(uint.MinValue, uint.MinValue, uint.MinValue)]
    [TestCase((uint)1, uint.MinValue, (uint)1)]
    [TestCase((uint)1, (uint)1, uint.MinValue)]
    [TestCase((uint)10, (uint)5, (uint)5)]
    [TestCase(uint.MaxValue, (uint)5, uint.MaxValue - 5)]
    public void GetSkippedTestCount_Should_Return_Correct_Skipped_Count(uint totalTest, uint executedTest, uint expectedSkippedTest)
    {
        // Arrange
        var counters = new Counters
        {
            Total = totalTest,
            Executed = executedTest
        };

        // Act
        var result = counters.GetSkippedTestCount();

        // Assert
        result.Should().Be(expectedSkippedTest);
    }
}